module CommandHandlers
open Queries
open Commands
open CommandHandlers
open Chessie.ErrorHandling

let getTabIdFromCommand = function
| OpenTab tab -> tab.Id
| _ -> failwith "TODO"

type Commander<'a, 'b> = {
  Validate : 'a -> Async<Choice<'b,string>>
  ToCommand : 'b -> Command
}

type ErrorResponse = {
  Message : string
}
let err msg = {Message = msg}

let handleCommand eventStore commandData handler = async {
  let! validationResult = handler.Validate commandData
  match validationResult with
  | Choice1Of2 validatedCommandData ->
    let command = handler.ToCommand validatedCommandData
    let! state = eventStore.GetState (getTabIdFromCommand command)
    match evolve state command with
    | Ok((newState, event), _) ->
      return (newState,event) |> ok
    | Bad (error) ->
      return error.Head |> sprintf "%A" |> err |> fail
  | Choice2Of2 errorMessage ->
    return errorMessage |> err |> fail
}