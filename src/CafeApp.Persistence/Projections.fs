module Projections
open Events
open Domain
open System

type TableActions = {
  OpenTab : Tab -> Async<unit>
  ReceivedOrder : Guid -> Async<unit>
  CloseTab : Tab -> Async<unit>
}

type ChefActions = {
  AddFoodsToPrepare : Guid -> Food list -> Async<unit>
  RemoveFood : Guid -> Food -> Async<unit>
  Remove : Guid -> Async<unit>
}

type WaiterActions = {
  AddDrinksToServe : Guid -> Drink list -> Async<unit>
  MarkDrinkServed : Guid -> Drink -> Async<unit>
  AddFoodToServe : Guid -> Food -> Async<unit>
  MarkFoodServed : Guid -> Food -> Async<unit>
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
  let tabId = order.Tab.Id
  [
    actions.Table.ReceivedOrder tabId
    actions.Chef.AddFoodsToPrepare tabId order.Foods
    actions.Waiter.AddDrinksToServe tabId order.Drinks
  ] |> Async.Parallel
| DrinkServed (item, tabId) ->
  [actions.Waiter.MarkDrinkServed tabId item]
  |> Async.Parallel
| FoodPrepared (item, tabId) ->
  [
    actions.Chef.RemoveFood tabId item
    actions.Waiter.AddFoodToServe tabId item
  ] |> Async.Parallel
| FoodServed (item, tabId) ->
  [actions.Waiter.MarkFoodServed tabId item]
  |> Async.Parallel
| OrderServed (order, payment) ->
  let tabId = order.Tab.Id
  [
    actions.Waiter.Remove tabId
    actions.Chef.Remove tabId
    actions.Cashier.AddTabAmount tabId payment.Amount
  ] |> Async.Parallel
| TabClosed payment ->
  let tabId = payment.Tab.Id
  [
    actions.Cashier.Remove tabId
    actions.Table.CloseTab payment.Tab
  ] |> Async.Parallel