module PlaceOrderTests

open NUnit.Framework
open CafeAppTestsDSL
open Domain
open System
open Commands
open Events
open Errors
open States

let tab = {Id = Guid.NewGuid(); TableNumber = 1}
let coke = DrinksItem {
            MenuNumber = 1
            Name = "Coke"
            Price = 1.5m}
let order = {
  TabId = tab.Id
  FoodItems = []
  DrinksItems = []
}
let salad = FoodItem {
  MenuNumber = 2
  Name = "Salad"
  Price = 2.5m
}


[<Test>]
let ``Can place drinks order`` () =
  let order = {order with DrinksItems = [coke]}
  Given (OpenedTab tab)
  |> When (PlaceOrder order)
  |> ThenStateShouldBe (PlacedOrder order)
  |> WithEvent (OrderPlaced order)

[<Test>]
let ``Can not place order with a closed tab`` () =
  let order = {order with DrinksItems = [coke]}
  Given (ClosedTab None)
  |> When (PlaceOrder order)
  |> ShouldFailWith CanNotOrderWithClosedTab

[<Test>]
let ``Can not place order multiple times`` () =
  let order = {order with DrinksItems = [coke]}
  Given (PlacedOrder order)
  |> When (PlaceOrder order)
  |> ShouldFailWith OrderAlreadyPlaced

[<Test>]
let ``Can place food order`` () =
  let order = {order with FoodItems = [salad]}
  Given (OpenedTab tab)
  |> When (PlaceOrder order)
  |> ThenStateShouldBe (PlacedOrder order)
  |> WithEvent (OrderPlaced order)

[<Test>]
let ``Can place food and drinks order`` () =
  let order = {order with
                  FoodItems = [salad]
                  DrinksItems = [coke]}
  Given (OpenedTab tab)
  |> When (PlaceOrder order)
  |> ThenStateShouldBe (PlacedOrder order)
  |> WithEvent (OrderPlaced order)