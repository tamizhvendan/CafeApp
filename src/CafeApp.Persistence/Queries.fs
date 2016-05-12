module Queries
open ReadModel
open Domain
open States
open System


type TableQueries = {
  GetTables : unit -> Async<Table list>
  GetTableByTableNumber : int -> Async<Table option>
  GetTableByTabId : Guid -> Async<Table option>
}

type FoodQueries = {
  GetFoods : unit -> Async<Food list>
  GetFoodByMenuNumber : int -> Async<Food option>
  GetFoodsByMenuNumbers : int[] -> Async<Choice<Food list, int[]>>
}

type DrinkQueries = {
  GetDrinks : unit -> Async<Drink list>
  GetDrinkByMenuNumber : int -> Async<Drink option>
  GetDrinksByMenuNumbers : int[] -> Async<Choice<Drink list, int[]>>
}

type ToDoQueries = {
  GetChefToDos : unit -> Async<ChefToDo list>
  GetWaiterToDos : unit -> Async<WaiterToDo list>
  GetCashierToDos : unit -> Async<Payment list>
}

type Queries = {
  Table : TableQueries
  Food : FoodQueries
  Drink : DrinkQueries
  ToDo : ToDoQueries
}