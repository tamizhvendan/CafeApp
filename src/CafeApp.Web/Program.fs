module Program

open Suave
open Suave.Web
open Suave.Successful
open Suave.RequestErrors
open Suave.Operators
open Suave.Filters
open CommandApi
open InMemory
open System.Text
open Chessie.ErrorHandling
open Events
open Projections
open JsonFormatter
open QueriesApi
open Suave.WebSocket
open Suave.Sockets.Control
open Suave.Sockets.SocketOp
open System.Reflection
open System.IO

let eventStream = new Control.Event<Event>()
let project event =
  projectReadModel inMemoryActions event
  |> Async.RunSynchronously |> ignore

let socketHandler (ws : WebSocket) cx = socket {
  while true do
    let! event =
      Control.Async.AwaitEvent(eventStream.Publish)
      |> Suave.Sockets.SocketOp.ofAsync
    let eventData =
      event |> eventJObj |> string |> Encoding.UTF8.GetBytes
    do! ws.send Text eventData true
}

let commandApiHandler eventStore (context : HttpContext) = async {
  let payload =
    Encoding.UTF8.GetString context.request.rawForm
  let! response =
    handleCommandRequest
      inMemoryQueries eventStore payload
  match response with
  | Ok ((state,events), _) ->
    for event in events do
      do! eventStore.SaveEvent state event
      eventStream.Trigger(event)
    return! toStateJson state context
  | Bad (err) ->
    return! toErrorJson err.Head context
}

let commandApi eventStore =
  path "/command"
    >=> POST
    >=> commandApiHandler eventStore

let clientDir =
  let exePath = Assembly.GetEntryAssembly().Location
  let exeDir = (new FileInfo(exePath)).Directory
  Path.Combine(exeDir.FullName, "public")

[<EntryPoint>]
let main argv =
  let app =
    let eventStore = inMemoryEventStore ()
    choose [
      path "/websocket" >=>
        handShake socketHandler
      commandApi eventStore
      queriesApi inMemoryQueries eventStore
      GET >=> choose [
        path "/" >=> Files.browseFileHome "index.html"
        Files.browseHome ]
    ]

  eventStream.Publish.Add(project)

  let cfg = {defaultConfig with
              homeFolder = Some(clientDir)
              bindings = [HttpBinding.mkSimple HTTP "0.0.0.0" 8083]
            }
  startWebServer cfg app
  0