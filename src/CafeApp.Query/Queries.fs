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
    GetFoodsByMenuNumbers : int[] -> Async<Choice<FoodItem list, int[]>>
    GetDrinksByMenuNumbers : int[] -> Async<Choice<DrinksItem list, int[]>>
    GetTableByTabId : Guid -> Async<Table option>
    GetDrinksByMenuNumber : int -> Async<DrinksItem option>
}