module CommandApi

open System.Text
open CommandHandlers
open Queries
open OpenTab
open PlaceOrder
open ServeDrinks
open PrepareFood
open ServeFood
open CloseTab
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
  | PrepareFoodRequest (tabId, foodMenuNumber) ->
    validationQueries.GetFoodByMenuNumber
    |> prepareFoodCommander
        validationQueries.GetTableByTabId
    |> handleCommand eventStore (tabId, foodMenuNumber)
  | ServeFoodRequest (tabId, foodMenuNumber) ->
    validationQueries.GetFoodByMenuNumber
    |> serveFoodCommander
        validationQueries.GetTableByTabId
    |> handleCommand eventStore (tabId, foodMenuNumber)
  | CloseTabRequest (tabId, amount) ->
    closeTabCommander validationQueries.GetTableByTabId
    |> handleCommand eventStore (tabId, amount)
  | _ -> err "Invalid command" |> fail |> async.Return