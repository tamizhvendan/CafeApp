module TestData
open Domain
open System

let tab = {Id = Guid.NewGuid(); TableNumber = 1}
let coke = Drink {
            MenuNumber = 1
            Name = "Coke"
            Price = 1.5m}
let lemonade = Drink {
            MenuNumber = 3
            Name = "Lemonade"
            Price = 1.0m}
let appleJuice = Drink {
            MenuNumber = 5
            Name = "Apple Juice"
            Price = 3.5m}
let order = {
  Tab = tab
  Foods = []
  Drinks = []
}
let salad = Food {
  MenuNumber = 2
  Name = "Salad"
  Price = 2.5m
}
let pizza = Food {
  MenuNumber = 4
  Name = "Pizza"
  Price = 6.5m
}

let foodPrice (Food food) = food.Price
let drinkPrice (Drink drink) = drink.Price