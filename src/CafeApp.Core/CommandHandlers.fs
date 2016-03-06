module CommandHandlers
open Chessie.ErrorHandling
open States
open Events
open System
open Domain
open Commands
open Errors

let handleOpenTab tab = function
| ClosedTab _ -> TabOpened tab |> ok
| _ -> TabAlreadyOpened |> fail

let handlePlaceOrder order = function
| OpenedTab _ -> OrderPlaced order |> ok
| ClosedTab _ -> fail CanNotOrderWithClosedTab
| _ -> fail OrderAlreadyPlaced

let execute state command =
  match command with
  | OpenTab tab -> handleOpenTab tab state
  | PlaceOrder order -> handlePlaceOrder order state
  | _ -> failwith "ToDo"

let evolve state command =
  match execute state command with
  | Ok (event,_) ->
    let newState = apply state event
    (newState, event) |> ok
  | Bad err -> Bad err