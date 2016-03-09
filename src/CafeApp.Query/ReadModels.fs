module ReadModel
open System
open Domain

type TableStatus = Open of Guid | Closed

type Table = {
  Number : int
  Waiter : string
  Status : TableStatus
}

type ChefToDo = {
  Tab : Tab
  FoodItems : FoodItem list
}

type WaiterToDo = {
  Tab : Tab
  FoodItems : FoodItem list
  DrinksItems : DrinksItem list
}