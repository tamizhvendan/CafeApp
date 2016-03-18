module CommandApiHandlers
open System.Text
open Suave
open Suave.Successful
open Suave.RequestErrors
open OpenTab
open Queries
open CommandHandlers
open Chessie.ErrorHandling
open Commands


let getTabIdFromCommand = function
| OpenTab tab -> tab.Id
| _ -> failwith "TODO"

let handleCommand validate toCommand validationQueries eventStore commandPayload = async {
  let! validationResult = validate validationQueries commandPayload
  match validationResult with
  | Choice1Of2 domainPayload ->
    let command = toCommand domainPayload
    let! state = eventStore.GetState (getTabIdFromCommand command)
    match evolve state command with
    | Ok((newState, event), _) ->
      return (newState,event) |> Choice1Of2
    | Bad (err) ->
      return err.Head |> sprintf "%A" |> Choice2Of2
  | Choice2Of2 errorMessage ->
    return Choice2Of2 errorMessage
}


let handleOpenTab eventStore validationQueries (context : HttpContext) tab = async {
  let! validationResult =
    validateOpenTab validationQueries.IsValidTableNumber tab
  match validationResult with
  | Choice1Of2 tab ->
      let command = toOpenTabCommand tab
      let! state = eventStore.GetState (tab.Id)
      match evolve state command with
      | Ok((newState, event), _) ->
        return OK (sprintf "%A" newState) context
      | Bad (err) ->
        return BAD_REQUEST (sprintf "%A" err.Head) context
  | Choice2Of2 errorMessage ->
    return BAD_REQUEST errorMessage context
}


let handleCommandRequest validationQueries (context : HttpContext) =
  let jsonPayload =
    Encoding.UTF8.GetString context.request.rawForm
  match jsonPayload with
  | OpenTabRequest tab -> failwith "TODO"
  | _ -> failwith "TODO"