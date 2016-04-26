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

type Food = Food of Item
type Drink = Drink of Item

type Payment = {
  Tab : Tab
  Amount : decimal
}

type Order = {
  Foods : Food list
  Drinks : Drink list
  Tab : Tab
}

type InProgressOrder = {
  PlacedOrder : Order
  ServedDrinks : Drink list
  ServedFoods : Food list
  PreparedFoods : Food list
}

let nonServedFoods ipo =
  List.except ipo.ServedFoods ipo.PlacedOrder.Foods

let nonServedDrinks ipo =
  List.except ipo.ServedDrinks ipo.PlacedOrder.Drinks

let isServingDrinkCompletesOrder ipo drink =
  List.isEmpty (nonServedFoods ipo) && (nonServedDrinks ipo) = [drink]

let isServingFoodCompletesOrder ipo food =
  List.isEmpty (nonServedDrinks ipo) && (nonServedFoods ipo) = [food]

let orderAmount order =
  let foodAmount =
    order.Foods
    |> List.map (fun (Food f) -> f.Price) |> List.sum
  let drinksAmount =
    order.Drinks
    |> List.map (fun (Drink d) -> d.Price) |> List.sum
  foodAmount + drinksAmount

let payment ipo =
  {Tab = ipo.PlacedOrder.Tab; Amount = orderAmount ipo.PlacedOrder}