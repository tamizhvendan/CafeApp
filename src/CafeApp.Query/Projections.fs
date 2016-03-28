module Projections
open Events
open Domain
open System

type TableActions = {
  OpenTab : Tab -> Async<unit>
  CloseTab : Tab -> Async<unit>
}

type ChefActions = {
  AddFoodItemsToPrepare : Guid -> FoodItem list -> Async<unit>
  RemoveFoodItem : Guid -> FoodItem -> Async<unit>
  Remove : Guid -> Async<unit>
}

type WaiterActions = {
  AddDrinksToServe : Guid -> DrinksItem list -> Async<unit>
  MarkDrinksServed : Guid -> DrinksItem -> Async<unit>
  AddFoodToServe : Guid -> FoodItem -> Async<unit>
  MarkFoodServed : Guid -> FoodItem -> Async<unit>
  Remove : Guid -> Async<unit>
}

type CashierActions = {
  AddTabAmount : Guid -> decimal -> Async<unit>
  Remove : Guid -> Async<unit>
}

type ProjectionActions = {
  Table : TableActions
  Cashier : CashierActions
  Waiter : WaiterActions
  Chef : ChefActions
}

let projectReadModel actions = function
| TabOpened tab ->
  [actions.Table.OpenTab tab] |> Async.Parallel
| OrderPlaced order ->
  let tabId = order.TabId
  [
    actions.Cashier.AddTabAmount order.TabId (orderAmount order)
    actions.Chef.AddFoodItemsToPrepare tabId order.FoodItems
    actions.Waiter.AddDrinksToServe tabId order.DrinksItems
  ] |> Async.Parallel
| DrinksServed (item, tabId) ->
  [actions.Waiter.MarkDrinksServed tabId item]
  |> Async.Parallel
| FoodPrepared (item, tabId) ->
  [
    actions.Chef.RemoveFoodItem tabId item
    actions.Waiter.AddFoodToServe tabId item
  ] |> Async.Parallel
| FoodServed (item, tabId) ->
  [actions.Waiter.MarkFoodServed tabId item]
  |> Async.Parallel
| TabClosed payment ->
  let tabId = payment.Tab.Id
  [
    actions.Cashier.Remove tabId
    actions.Waiter.Remove tabId
    actions.Chef.Remove tabId
    actions.Table.CloseTab payment.Tab
  ] |> Async.Parallel