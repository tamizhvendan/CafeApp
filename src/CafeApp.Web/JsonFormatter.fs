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
open Events

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
let foodJObj (Food item) = itemJObj item
let drinkJObj (Drink item) = itemJObj item
let foodJArray foods =
  foods |> List.map foodJObj |> jArray
let drinkJArray drinks =
  drinks |> List.map drinkJObj |> jArray

let orderJObj (order : Order) =
  jobj [
    "tabId" .= order.Tab.Id
    "tableNumber" .= order.Tab.TableNumber
    "foods" .= foodJArray order.Foods
    "drinks" .= drinkJArray order.Drinks
  ]

let orderInProgressJObj ipo =
  jobj [
    "tabId" .=  ipo.PlacedOrder.Tab.Id
    "tableNumber" .= ipo.PlacedOrder.Tab.TableNumber
    "preparedFoods" .= foodJArray ipo.PreparedFoods
    "servedFoods" .= foodJArray ipo.ServedFoods
    "servedDrinks" .= drinkJArray ipo.ServedDrinks
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
| ServedOrder order ->
  jobj [
    "state" .= "ServedOrder"
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
| InService tabId ->
  "status" .= jobj [
                "inService" .= tabId.ToString()
              ]
| Closed -> "status" .= "closed"

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
    "foods" .= foodJArray todo.Foods
  ]

let toChefToDosJSON =
  toReadModelsJson chefToDoJObj "chefToDos"

let waiterToDoJObj todo =
  jobj [
    "tabId" .= todo.Tab.Id.ToString()
    "tableNumber" .= todo.Tab.TableNumber
    "foods" .= foodJArray todo.Foods
    "drinks" .= drinkJArray todo.Drinks
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

let toFoodsJSON =
  toReadModelsJson foodJObj "foods"

let toDrinksJSON =
  toReadModelsJson drinkJObj "drinks"

let eventJObj = function
| TabOpened tab ->
  jobj [
    "event" .= "TabOpened"
    "data" .= tabJObj tab
  ]
| OrderPlaced order ->
  jobj [
    "event" .= "OrderPlaced"
    "data" .= jobj [
      "order" .= orderJObj order
      "amount" .= orderAmount order
    ]
  ]
| DrinkServed (item, tabId) ->
  jobj [
    "event" .= "DrinkServed"
    "data" .= jobj [
      "drink" .= drinkJObj item
      "tabId" .= tabId
    ]
  ]
| FoodPrepared (item,tabId) ->
  jobj [
    "event" .= "FoodPrepared"
    "data" .= jobj [
      "food" .= foodJObj item
      "tabId" .= tabId
    ]
  ]
| FoodServed (item, tabId) ->
  jobj [
    "event" .= "FoodServed"
    "data" .= jobj [
      "food" .= foodJObj item
      "tabId" .= tabId
    ]
  ]
| OrderServed (order, payment) ->
  jobj [
    "event" .= "OrderServed"
    "data" .= jobj [
      "order" .= orderJObj order
      "tabId" .= payment.Tab.Id
      "tableNumber" .= payment.Tab.TableNumber
      "amount" .= payment.Amount
    ]
  ]
| TabClosed payment ->
  jobj [
    "event" .= "TabClosed"
    "data" .= jobj [
      "amountPaid" .= payment.Amount
      "tabId" .= payment.Tab.Id
      "tableNumber" .= payment.Tab.TableNumber
    ]
  ]