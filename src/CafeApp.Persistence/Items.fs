module Items
open System.Collections.Generic
open Domain

let private foodItems =
  let dict = new Dictionary<int, FoodItem>()
  dict.Add(8, FoodItem {
    MenuNumber = 8
    Price = 5m
    Name = "Salad"
  })
  dict.Add(9, FoodItem {
    MenuNumber = 9
    Price = 10m
    Name = "Pizza"
  })
  dict

let private drinksItems =
  let dict = new Dictionary<int, DrinksItem>()
  dict.Add(10, DrinksItem {
      MenuNumber = 10
      Price = 2.5m
      Name = "Coke"
  })
  dict.Add(11, DrinksItem {
    MenuNumber = 11
    Name = "Lemonade"
    Price = 1.5m
  })
  dict

let private getItems<'a> (dict : Dictionary<int,'a>) keys =
  let invalidKeys = keys |> Array.except dict.Keys
  if Array.isEmpty invalidKeys then
    keys
    |> Array.map (fun n -> dict.[n])
    |> Array.toList
    |> Choice1Of2
  else
    invalidKeys |> Choice2Of2

let getItem<'a> (dict : Dictionary<int,'a>) key =
  if dict.ContainsKey key then
    dict.[key] |> Some
  else
    None

let getFoodsByMenuNumbers keys =
  getItems foodItems keys |> async.Return

let getFoodByMenuNumber key =
  getItem foodItems key |> async.Return

let getDrinksByMenuNumbers keys =
  getItems drinksItems keys |> async.Return

let getDrinksByMenuNumber key =
  getItem drinksItems key |> async.Return

let getFoodItems () =
  foodItems.Values |> Seq.toList |> async.Return

let getDrinksItems () =
  drinksItems.Values |> Seq.toList |> async.Return