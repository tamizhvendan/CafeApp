module InMemory
open Table
open Chef
open Waiter
open Cashier
open Projections
open Queries
open Items
open EventStore
open NEventStore

type InMemoryEventStore () =
  static member Instance =
                  Wireup.Init()
                    .UsingInMemoryPersistence()
                    .Build()

let inMemoryEventStore () =
  let eventStoreInstance = InMemoryEventStore.Instance
  {
    GetState = getState eventStoreInstance
    SaveEvent = saveEvent eventStoreInstance
  }

let inMemoryQueries = {
  GetTables = getTables
  GetChefToDos = getChefToDos
  GetCashierToDos = getCashierToDos
  GetWaiterToDos = getWaiterToDos
  GetDrinksItems = getDrinksItems
  GetFoodItems = getFoodItems
}

let inMemoryValidationQueries =
  let getTableByTabId tabId =
    getTableByTabId tabId |> async.Return

  {
    GetTableByTableNumber = getTableByTableNumber
    GetTableByTabId = getTableByTabId
    GetFoodsByMenuNumbers = getFoodsByMenuNumbers
    GetDrinksByMenuNumbers = getDrinksByMenuNumbers
    GetDrinksByMenuNumber = getDrinksByMenuNumber
    GetFoodByMenuNumber = getFoodByMenuNumber
  }

let inMemoryActions = {
  Table = tableActions
  Chef = chefActions
  Waiter = waiterActions
  Cashier = cashierActions
}