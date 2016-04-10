module CommandHandlers
open Queries
open EventStore
open Commands
open CommandHandlers
open Errors
open Chessie.ErrorHandling

let getTabIdFromCommand = function
| OpenTab tab -> tab.Id
| PlaceOrder order -> order.Tab.Id
| ServeDrinks (item, tabId) -> tabId
| PrepareFood (item, tabId) -> tabId
| ServeFood (item, tabId) -> tabId
| CloseTab payment -> payment.Tab.Id

type Commander<'a, 'b> = {
  Validate : 'a -> Async<Choice<'b,string>>
  ToCommand : 'b -> Command
}

type ErrorResponse = {
  Message : string
}
let err msg = {Message = msg}

let handleCommand eventStore commandData commander = async {
  let! validationResult = commander.Validate commandData
  match validationResult with
  | Choice1Of2 validatedCommandData ->
    let command = commander.ToCommand validatedCommandData
    let! state = eventStore.GetState (getTabIdFromCommand command)
    match evolve state command with
    | Ok((newState, event), _) ->
      return (newState,event) |> ok
    | Bad (error) ->
      return error.Head |> toErrorString |> err |> fail
  | Choice2Of2 errorMessage ->
    return errorMessage |> err |> fail
}