module Queries
open ReadModel
open Domain
open States
open System

type Queries = {
  GetTables : unit -> Async<Table list>
  GetChefToDos : unit -> Async<ChefToDo list>
  GetCashierToDos : unit -> Async<Payment list>
}

type ValidationQueries = {
    GetTableByTableNumber : int -> Async<Table option>
}