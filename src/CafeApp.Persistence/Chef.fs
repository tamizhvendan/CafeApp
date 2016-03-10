module Chef
open System.Collections.Generic
open Domain
open ReadModel
open System
open Table
open Projections

let private chefToDos = new Dictionary<Guid, ChefToDo>()

let private addFoodItemsToPrepare tabId foodItems =
  match getTableByTabId tabId with
  | Some table ->
    let tab = {Id = tabId; TableNumber = table.Number}
    let todo : ChefToDo = {Tab = tab; FoodItems = foodItems}
    chefToDos.Add(tabId, todo)
  | None -> ()
  async.Return ()

let private removeFoodItem tabId foodItem =
  let todo = chefToDos.[tabId]
  let chefToDo =
    { todo with FoodItems =
                  List.filter (fun d -> d <> foodItem) todo.FoodItems}
  chefToDos.[tabId] <- chefToDo
  async.Return ()

let private remove tabId =
  chefToDos.Remove(tabId) |> ignore
  async.Return ()

let chefActions = {
  AddFoodItemsToPrepare = addFoodItemsToPrepare
  RemoveFoodItem = removeFoodItem
  Remove = remove
}

let getChefToDos () =
  chefToDos.Values
  |> Seq.toList
  |> async.Return