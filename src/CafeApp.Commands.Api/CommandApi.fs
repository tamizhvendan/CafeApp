module CommandApi

open System.Text
open CommandHandlers
open Queries
open OpenTab
open PlaceOrder
open ServeDrink
open PrepareFood
open ServeFood
open CloseTab
open Chessie.ErrorHandling

let handleCommandRequest queries eventStore
  = function
  | OpenTabRequest tab ->
    queries.Table.GetTableByTableNumber
    |> openTabCommander
    |> handleCommand eventStore tab
  | PlaceOrderRequest placeOrder ->
    placeOrderCommander queries
    |> handleCommand eventStore placeOrder
  | ServeDrinkRequest (tabId, drinksMenuNumber) ->
    queries.Drink.GetDrinkByMenuNumber
    |> serveDrinkCommander
        queries.Table.GetTableByTabId
    |> handleCommand eventStore (tabId, drinksMenuNumber)
  | PrepareFoodRequest (tabId, foodMenuNumber) ->
    queries.Food.GetFoodByMenuNumber
    |> prepareFoodCommander
        queries.Table.GetTableByTabId
    |> handleCommand eventStore (tabId, foodMenuNumber)
  | ServeFoodRequest (tabId, foodMenuNumber) ->
    queries.Food.GetFoodByMenuNumber
    |> serveFoodCommander
        queries.Table.GetTableByTabId
    |> handleCommand eventStore (tabId, foodMenuNumber)
  | CloseTabRequest (tabId, amount) ->
    closeTabCommander queries.Table.GetTableByTabId
    |> handleCommand eventStore (tabId, amount)
  | _ -> err "Invalid command" |> fail |> async.Return