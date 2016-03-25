module CommandApi

open System.Text
open CommandHandlers
open OpenTab
open PlaceOrder
open Queries
open Chessie.ErrorHandling

let handleCommandRequest validationQueries eventStore
  = function
  | OpenTabRequest tab ->
    validationQueries.GetTableByTableNumber
    |> openTabCommander
    |> handleCommand eventStore tab
  | PlaceOrderRequest placeOrder ->
    placeOrderCommander validationQueries
    |> handleCommand eventStore placeOrder
  | _ -> err "Invalid command" |> fail |> async.Return