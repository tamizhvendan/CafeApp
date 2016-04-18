module Queries
open ReadModel
open Domain
open States
open System

type Queries = {
  GetTables : unit -> Async<Table list>
  GetChefToDos : unit -> Async<ChefToDo list>
  GetWaiterToDos : unit -> Async<WaiterToDo list>
  GetCashierToDos : unit -> Async<Payment list>
  GetFoods : unit -> Async<Food list>
  GetDrinks : unit -> Async<Drink list>
}

type ValidationQueries = {
    GetTableByTableNumber : int -> Async<Table option>
    GetFoodsByMenuNumbers : int[] -> Async<Choice<Food list, int[]>>
    GetDrinksByMenuNumbers : int[] -> Async<Choice<Drink list, int[]>>
    GetTableByTabId : Guid -> Async<Table option>
    GetDrinkByMenuNumber : int -> Async<Drink option>
    GetFoodByMenuNumber : int -> Async<Food option>
}