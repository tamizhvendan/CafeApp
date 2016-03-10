module InMemory
open Table
open Chef
open Waiter
open Cashier
open Projections
open Queries
open EventStore
open NEventStore

let private inMemoryEventStoreInstance =
              Wireup.Init()
                .UsingInMemoryPersistence()
                .Build()

let inMemoryEventStore = {
  GetState = getState inMemoryEventStoreInstance
  SaveEvent = saveEvent inMemoryEventStoreInstance
}

let inMemoryQueries = {
  GetTables = getTables
  GetChefToDos = getChefToDos
  GetCashierToDos = getCashierToDos
  GetState = inMemoryEventStore.GetState
}

let inMemoryActions = {
  Table = tableActions
  Chef = chefActions
  Waiter = waiterActions
  Cashier = cashierActions
}