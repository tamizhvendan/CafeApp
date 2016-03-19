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

[<EntryPoint>]
let main argv =

  let commandApiHandler eventStore (context : HttpContext) = async {
    let payload =
      Encoding.UTF8.GetString context.request.rawForm
    let! response =
      handleCommandRequest
        inMemoryValidationQueries eventStore payload
    match response with
    | Ok ((state,event), _) ->
      return! OK (sprintf "%A" state) context
    | Bad (err) ->
      return! BAD_REQUEST err.Head.Message context
  }

  let commandApi eventStore =
    path "/command"
      >=> POST
      >=> commandApiHandler eventStore


  let app =
    let eventStore = inMemoryEventStore ()
    choose [
      commandApi eventStore
    ]

  startWebServer defaultConfig app
  0