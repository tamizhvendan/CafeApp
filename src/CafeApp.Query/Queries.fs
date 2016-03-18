module Queries
open ReadModel
open Domain
open States
open System

type Queries = {
  GetTables : unit -> Async<Table list>
  GetChefToDos : unit -> Async<ChefToDo list>
  GetCashierToDos : unit -> Async<Payment list>
  GetState : Guid -> Async<State>
}

type ValidationQueries = {
    IsValidTableNumber : int -> Async<bool>
}