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

type CommandHandler<'a, 'b> = {
  Validate : ValidationQueries -> 'a -> Async<Choice<'b,string>>
  ToCommand : 'b -> Command
}

let openTabHandler = {
  Validate = validateOpenTab
  ToCommand = toOpenTabCommand
}

let handleCommand validationQueries eventStore commandData handler  = async {
  let! validationResult = handler.Validate validationQueries commandData
  match validationResult with
  | Choice1Of2 validatedCommandData ->
    let command = handler.ToCommand validatedCommandData
    let! state = eventStore.GetState (getTabIdFromCommand command)
    match evolve state command with
    | Ok((newState, event), _) ->
      return (newState,event) |> Choice1Of2
    | Bad (err) ->
      return err.Head |> sprintf "%A" |> Choice2Of2
  | Choice2Of2 errorMessage ->
    return Choice2Of2 errorMessage
}

let handleCommandRequest validationQueries eventStore (context : HttpContext) =
  let jsonPayload =
    Encoding.UTF8.GetString context.request.rawForm

  let handle = handleCommand validationQueries eventStore

  match jsonPayload with
  | OpenTabRequest tab -> handle tab openTabHandler
  | _ -> failwith "TODO"