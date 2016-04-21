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
open System.Reactive.Linq
open System.Reactive.Subjects
open System.Reactive.Concurrency
open Projections
open JsonFormatter
open QueriesApi
open Suave.WebSocket
open Suave.Sockets.Control
open Suave.Sockets.SocketOp
open AsyncHelpers
open System.Reflection
open System.IO

let eventStream = new Subject<Event>()
let asyncEventStream =
  eventStream.ObserveOn(Scheduler.Default)

let project event =
  projectReadModel inMemoryActions event
  |> Async.RunSynchronously |> ignore


let socketOfObservable eventStream (ws : WebSocket) cx = socket {
  while true do
    let! event =
      eventStream
      |> Async.AwaitObservable
      |> ofAsync
    let eventData =
      event |> eventJObj |> string |> Encoding.UTF8.GetBytes
    do! ws.send Text eventData true
}

let commandApiHandler eventStore (context : HttpContext) = async {
  let payload =
    Encoding.UTF8.GetString context.request.rawForm
  let! response =
    handleCommandRequest
      inMemoryValidationQueries eventStore payload
  match response with
  | Ok ((state,events), _) ->
    for event in events do
      do! eventStore.SaveEvent state event
      eventStream.OnNext(event)
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
        handShake (socketOfObservable asyncEventStream)
      commandApi eventStore
      queriesApi inMemoryQueries eventStore
      GET >=> choose [
        path "/" >=> Files.browseFileHome "index.html"
        Files.browseHome ]
    ]

  use projectionSubscription =
    asyncEventStream.Subscribe project

  let cfg = {defaultConfig with
              homeFolder = Some(clientDir)
              bindings = [HttpBinding.mkSimple HTTP "0.0.0.0" 8083]
            }
  startWebServer cfg app
  0