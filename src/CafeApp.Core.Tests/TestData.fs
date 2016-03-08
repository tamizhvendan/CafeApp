module TestData
open Domain
open System

let tab = {Id = Guid.NewGuid(); TableNumber = 1}
let coke = DrinksItem {
            MenuNumber = 1
            Name = "Coke"
            Price = 1.5m}
let lemonade = DrinksItem {
            MenuNumber = 3
            Name = "Lemonade"
            Price = 1.0m}
let appleJuice = DrinksItem {
            MenuNumber = 5
            Name = "Apple Juice"
            Price = 3.5m}
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
let pizza = FoodItem {
  MenuNumber = 4
  Name = "Pizza"
  Price = 6.5m
}