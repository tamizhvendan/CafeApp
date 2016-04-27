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
        Foods = []
        Drinks = drinksItems}
    waiterToDos.Add(tabId, todo)
  | None -> ()
  async.Return ()

let private addFoodToServe tabId food  =
  if waiterToDos.ContainsKey tabId then
    let todo = waiterToDos.[tabId]
    let waiterToDo =
      {todo with Foods = food :: todo.Foods}
    waiterToDos.[tabId] <- waiterToDo
  else
    match getTableByTabId tabId with
    | Some table ->
      let todo =
        { Tab = {Id = tabId; TableNumber = table.Number}
          Foods = [food]
          Drinks = []}
      waiterToDos.Add(tabId, todo)
    | None -> ()
  async.Return ()

let private markDrinkServed tabId drink =
  let todo = waiterToDos.[tabId]
  let waiterToDo =
    { todo with
        Drinks =
          List.filter (fun d -> d <> drink) todo.Drinks }
  waiterToDos.[tabId] <- waiterToDo
  async.Return ()

let private markFoodServed tabId food =
  let todo = waiterToDos.[tabId]
  let waiterToDo =
    { todo with
        Foods =
          List.filter (fun d -> d <> food) todo.Foods }
  waiterToDos.[tabId] <- waiterToDo
  async.Return ()

let private remove tabId =
  waiterToDos.Remove(tabId) |> ignore
  async.Return ()


let waiterActions = {
  AddDrinksToServe = addDrinksToServe
  MarkDrinkServed = markDrinkServed
  AddFoodToServe = addFoodToServe
  MarkFoodServed = markFoodServed
  Remove = remove
}

let getWaiterToDos () =
  waiterToDos.Values
  |> Seq.toList
  |> async.Return
