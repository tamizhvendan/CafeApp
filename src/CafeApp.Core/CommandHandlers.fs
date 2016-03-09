module CommandHandlers
open Chessie.ErrorHandling
open States
open Events
open System
open Domain
open Commands
open Errors

let handleOpenTab tab = function
| ClosedTab _ -> TabOpened tab |> ok
| _ -> TabAlreadyOpened |> fail

let handlePlaceOrder order = function
| OpenedTab _ ->
  if List.isEmpty order.FoodItems && List.isEmpty order.DrinksItems then
    fail CanNotPlaceEmptyOrder
  else
    OrderPlaced order |> ok
| ClosedTab _ -> fail CanNotOrderWithClosedTab
| _ -> fail OrderAlreadyPlaced

let handleServeDrinks item tabId state =
  let isOrderedDrinks item order =
    List.contains item order.DrinksItems
  match state with
  | PlacedOrder order ->
      if isOrderedDrinks item order then
        DrinksServed (item,tabId) |> ok
      else
        CanNotServeNonOrderedDrinks item |> fail
  | OrderServed _ -> OrderAlreadyServed |> fail
  | OpenedTab _ ->  CanNotServeForNonPlacedOrder |> fail
  | ClosedTab _ -> CanNotServeWithClosedTab |> fail
  | OrderInProgress ipo ->
      if isOrderedDrinks item ipo.PlacedOrder then
        let nonServedDrinks = nonServedDrinks ipo
        if List.contains item nonServedDrinks then
          DrinksServed (item,tabId) |> ok
        else
          CanNotServeAlreadyServedDrinks item |> fail
      else
        CanNotServeNonOrderedDrinks item |> fail

let handlePrepareFood item tabId state =
  let isOrderedFoodItem item order =
    List.contains item order.FoodItems
  match state with
  | PlacedOrder order ->
    if isOrderedFoodItem item order then
      FoodPrepared (item, tabId) |> ok
    else
      CanNotPrepareNonOrderedFood item |> fail
  | OrderServed _ -> OrderAlreadyServed |> fail
  | OpenedTab _ ->  CanNotPrepareForNonPlacedOrder |> fail
  | ClosedTab _ -> CanNotPrepareWithClosedTab |> fail
  | OrderInProgress ipo ->
    if isOrderedFoodItem item ipo.PlacedOrder then
      if List.contains item ipo.PreparedFoods then
        CanNotPrepareAlreadyPreparedFood item |> fail
      else
        FoodPrepared (item, tabId) |> ok
    else
      CanNotPrepareNonOrderedFood item |> fail

let handleServeFood item tabId = function
| OrderInProgress ipo ->
  if List.contains item ipo.PlacedOrder.FoodItems then
    if List.contains item ipo.ServedFoods then
      CanNotServeAlreadyServedFood item |> fail
    else
      if List.contains item ipo.PreparedFoods then
        FoodServed (item, tabId) |> ok
      else
        CanNotServeNonPreparedFood item |> fail
  else
    CanNotServeNonOrderedFood item |> fail
| PlacedOrder _ -> CanNotServeNonPreparedFood item |> fail
| OrderServed _ -> OrderAlreadyServed |> fail
| OpenedTab _ -> CanNotServeForNonPlacedOrder |> fail
| ClosedTab _ -> CanNotServeWithClosedTab |> fail

let handleCloseTab payment = function
| OrderServed order ->
  let orderAmount = orderAmount order
  if payment.Amount = orderAmount then
    TabClosed payment |> ok
  else
    InvalidPayment (orderAmount, payment.Amount) |> fail
| _ -> CanNotPayForNonServedOrder |> fail

let execute state command =
  match command with
  | OpenTab tab -> handleOpenTab tab state
  | PlaceOrder order -> handlePlaceOrder order state
  | ServeDrinks (item, tabId) -> handleServeDrinks item tabId state
  | PrepareFood (item, tabId) -> handlePrepareFood item tabId state
  | ServeFood (item, tabId) -> handleServeFood item tabId state
  | CloseTab payment -> handleCloseTab payment state

let evolve state command =
  match execute state command with
  | Ok (event,_) ->
    let newState = apply state event
    (newState, event) |> ok
  | Bad err -> Bad err