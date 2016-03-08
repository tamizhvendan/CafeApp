module Domain
open System

type Tab = {
  Id : Guid
  TableNumber : int
}

type Item = {
  MenuNumber : int
  Price : decimal
  Name : string
}

type FoodItem = FoodItem of Item
type DrinksItem = DrinksItem of Item

type Payment = {
  Tab : Tab
  Amount : decimal
}

type Order = {
  FoodItems : FoodItem list
  DrinksItems : DrinksItem list
  TabId : Guid
}

type InProgressOrder = {
  PlacedOrder : Order
  ServedDrinks : DrinksItem list
  ServedFoods : FoodItem list
  PreparedFoods : FoodItem list
}

let nonServedFoods ipo =
  List.except ipo.ServedFoods ipo.PlacedOrder.FoodItems

let nonServedDrinks ipo =
  List.except ipo.ServedDrinks ipo.PlacedOrder.DrinksItems

let isOrderServed ipo =
  List.isEmpty (nonServedFoods ipo) &&
    List.isEmpty (nonServedDrinks ipo)