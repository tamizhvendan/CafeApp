module InMemory
open Table
open Chef
open Waiter
open Cashier
open Projections
open Queries
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
}

let inMemoryValidationQueries = {
  GetTableByTableNumber = getTableByTableNumber
}

let inMemoryActions = {
  Table = tableActions
  Chef = chefActions
  Waiter = waiterActions
  Cashier = cashierActions
}