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

let eventStream = new Subject<Event>()
let asyncEventStream =
  eventStream.ObserveOn(Scheduler.Default)

let project event =
  projectReadModel inMemoryActions event
  |> Async.RunSynchronously |> ignore



let commandApiHandler eventStore (context : HttpContext) = async {
  let payload =
    Encoding.UTF8.GetString context.request.rawForm
  let! response =
    handleCommandRequest
      inMemoryValidationQueries eventStore payload
  match response with
  | Ok ((state,event), _) ->
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

[<EntryPoint>]
let main argv =

  let app =
    let eventStore = inMemoryEventStore ()
    choose [
      commandApi eventStore
    ]

  use projectionSubscription =
    asyncEventStream.Subscribe project

  startWebServer defaultConfig app
  0