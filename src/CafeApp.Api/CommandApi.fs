module CommandApi

open System.Text
open CommandHandlers
open OpenTab
open PlaceOrder
open Queries
open ServeDrinks
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
  | ServeDrinksRequest (tabId, drinksMenuNumber) ->
    validationQueries.GetDrinksByMenuNumber
    |> serveDrinksCommander
        validationQueries.GetTableByTabId
    |> handleCommand eventStore (tabId, drinksMenuNumber)
  | _ -> err "Invalid command" |> fail |> async.Return