module Waiter
open Projections
open ReadModel
open System.Collections.Generic
open System
open Table

let private waiterToDos = new Dictionary<Guid, WaiterToDo>()

let private addDrinksToServe tabId drinksItems =
  match getTableByTabId tabId with
  | Some table ->
    let todo =
      { Tab = {Id = tabId; TableNumber = table.Number}
        FoodItems = []
        DrinksItems = drinksItems}
    waiterToDos.Add(tabId, todo)
  | None -> ()
  async.Return ()

let private addFoodToServe tabId item  =
  if waiterToDos.ContainsKey tabId then
    let todo = waiterToDos.[tabId]
    let waiterToDo =
      {todo with FoodItems = item :: todo.FoodItems}
    waiterToDos.[tabId] <- waiterToDo
  else
    match getTableByTabId tabId with
    | Some table ->
      let todo =
        { Tab = {Id = tabId; TableNumber = table.Number}
          FoodItems = [item]
          DrinksItems = []}
      waiterToDos.Add(tabId, todo)
    | None -> ()
  async.Return ()

let private markDrinksServed tabId drinks =
  let todo = waiterToDos.[tabId]
  let waiterToDo =
    { todo with
        DrinksItems =
          List.filter (fun d -> d <> drinks) todo.DrinksItems }
  waiterToDos.[tabId] <- waiterToDo
  async.Return ()

let private markFoodServed tabId food =
  let todo = waiterToDos.[tabId]
  let waiterToDo =
    { todo with
        FoodItems =
          List.filter (fun d -> d <> food) todo.FoodItems }
  waiterToDos.[tabId] <- waiterToDo
  async.Return ()

let private remove tabId =
  waiterToDos.Remove(tabId) |> ignore
  async.Return ()


let waiterActions = {
  AddDrinksToServe = addDrinksToServe
  MarkDrinksServed = markDrinksServed
  AddFoodToServe = addFoodToServe
  MarkFoodServed = markFoodServed
  Remove = remove
}

let getWaiterToDos () =
  waiterToDos.Values
  |> Seq.toList
  |> async.Return
