module JsonFormatter
open Suave
open Suave.Successful
open Suave.Operators
open Newtonsoft.Json.Linq
open Domain
open States

let (.=) key (value : obj) = new JProperty(key, value)

let jobj jProperties =
  let jObject = new JObject()
  jProperties |> List.iter jObject.Add
  jObject

let jArray jObjects =
  let jArray = new JArray()
  jObjects |> List.iter jArray.Add
  jArray

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
    jobj [ state; "tabId" .= id.ToString() ]
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

let JSON jsonString (context : HttpContext) = async {
  let wp =
    OK jsonString >=>
      Writers.setMimeType "application/json; charset=utf-8"
  return! wp context
}

let toStateJson state =
  state |> stateJObj |> string |> JSON