module JsonFormatter
open Suave
open Suave.Successful
open Suave.Operators
open Newtonsoft.Json.Linq
open Domain
open States
open CommandHandlers
open Suave.RequestErrors
open ReadModel

let (.=) key (value : obj) = new JProperty(key, value)

let jobj jProperties =
  let jObject = new JObject()
  jProperties |> List.iter jObject.Add
  jObject

let jArray jObjects =
  let jArray = new JArray()
  jObjects |> List.iter jArray.Add
  jArray

let tabIdJObj tabId =
  jobj [
    "tabId" .= tabId
  ]

let tabJObj tab =
  jobj [
    "id" .= tab.Id
    "tableNumber" .= tab.TableNumber
  ]
let itemJObj item =
  jobj [
    "menuNumber" .= item.MenuNumber
    "name" .= item.Name
  ]
let foodItemJObj (FoodItem item) = itemJObj item
let drinksItemJObj (DrinksItem item) = itemJObj item
let foodItemsJArray foodItems =
  foodItems |> List.map foodItemJObj |> jArray
let drinksItemsJArray drinksItems =
  drinksItems |> List.map drinksItemJObj |> jArray

let orderJObj order =
  jobj [
    "tabId" .= order.TabId
    "foodItems" .= foodItemsJArray order.FoodItems
    "drinksItems" .= drinksItemsJArray order.DrinksItems
  ]

let orderInProgressJObj ipo =
  jobj [
    "tabId" .=  ipo.PlacedOrder.TabId.ToString()
    "preparedFoods" .= foodItemsJArray ipo.PreparedFoods
    "servedFoods" .= foodItemsJArray ipo.ServedFoods
    "servedDrinks" .= drinksItemsJArray ipo.ServedDrinks
  ]

let stateJObj = function
| ClosedTab tabId ->
  let state = "state" .= "ClosedTab"
  match tabId with
  | Some id ->
    jobj [ state; "data" .= tabIdJObj id ]
  | None -> jobj [state]
| OpenedTab tab ->
  jobj [
    "state" .= "OpenedTab"
    "data" .= tabJObj tab
  ]
| PlacedOrder order ->
  jobj [
    "state" .= "PlacedOrder"
    "data" .= orderJObj order
  ]
| OrderInProgress ipo ->
  jobj [
    "state" .= "OrderInProgress"
    "data" .= orderInProgressJObj ipo
  ]
| OrderServed order ->
  jobj [
    "state" .= "OrderServed"
    "data" .= orderJObj order
  ]

let JSON webpart jsonString (context : HttpContext) = async {
  let wp =
    webpart jsonString >=>
      Writers.setMimeType
        "application/json; charset=utf-8"
  return! wp context
}

let toStateJson state =
  state |> stateJObj |> string |> JSON OK

let toErrorJson err =
  jobj [ "error" .= err.Message]
  |> string |> JSON BAD_REQUEST

let statusJObj = function
| Open tabId ->
  "status" .= jobj [
                "open" .= tabId.ToString()
              ]
| _ -> "status" .= "closed"

let tableJObj table =
  jobj [
    "number" .= table.Number
    "waiter" .= table.Waiter
    statusJObj table.Status
  ]

let toReadModelsJson toJObj key models =
  models
  |> List.map toJObj |> jArray
  |> (.=) key |> List.singleton |> jobj
  |> string |> JSON OK

let toTablesJSON = toReadModelsJson tableJObj "tables"

let chefToDoJObj (todo : ChefToDo) =
  jobj [
    "tabId" .= todo.Tab.Id.ToString()
    "tableNumber" .= todo.Tab.TableNumber
    "foodItems" .= foodItemsJArray todo.FoodItems
  ]

let toChefToDosJSON =
  toReadModelsJson chefToDoJObj "chefToDos"

let waiterToDoJObj todo =
  jobj [
    "tabId" .= todo.Tab.Id.ToString()
    "tableNumber" .= todo.Tab.TableNumber
    "foodItems" .= foodItemsJArray todo.FoodItems
    "drinksItems" .= drinksItemsJArray todo.DrinksItems
  ]

let toWaiterToDosJSON =
  toReadModelsJson waiterToDoJObj "waiterToDos"

let cashierToDoJObj (payment : Payment) =
  jobj [
    "tabId" .= payment.Tab.Id.ToString()
    "tableNumber" .= payment.Tab.TableNumber
    "paymentAmount" .= payment.Amount
  ]

let toCashierToDosJSON =
  toReadModelsJson cashierToDoJObj "cashierToDos"