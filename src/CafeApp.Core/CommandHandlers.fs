module CommandHandlers
open Chessie.ErrorHandling
open States
open Events
open System
open Domain
open Commands
open Errors

let toList x = [x]
let toListOK = toList >> ok

let handleOpenTab tab = function
| ClosedTab _ -> TabOpened tab |> toListOK
| _ -> TabAlreadyOpened |> fail

let handlePlaceOrder order = function
| OpenedTab _ ->
  if List.isEmpty order.Foods && List.isEmpty order.Drinks then
    fail CanNotPlaceEmptyOrder
  else
    OrderPlaced order |> toListOK
| ClosedTab _ -> fail CanNotOrderWithClosedTab
| _ -> fail OrderAlreadyPlaced

let handleServeDrink item tabId state =
  let isOrderedDrinks item order =
    List.contains item order.Drinks
  match state with
  | PlacedOrder order ->
      if isOrderedDrinks item order then
        DrinkServed (item,tabId) |> toListOK
      else
        CanNotServeNonOrderedDrink item |> fail
  | OrderServed _ -> OrderAlreadyServed |> fail
  | OpenedTab _ ->  CanNotServeForNonPlacedOrder |> fail
  | ClosedTab _ -> CanNotServeWithClosedTab |> fail
  | OrderInProgress ipo ->
      if isOrderedDrinks item ipo.PlacedOrder then
        let nonServedDrinks = nonServedDrinks ipo
        if List.contains item nonServedDrinks then
          DrinkServed (item,tabId) |> toListOK
        else
          CanNotServeAlreadyServedDrink item |> fail
      else
        CanNotServeNonOrderedDrink item |> fail

let handlePrepareFood item tabId state =
  let isOrderedFoodItem item order =
    List.contains item order.Foods
  match state with
  | PlacedOrder order ->
    if isOrderedFoodItem item order then
      FoodPrepared (item, tabId) |> toListOK
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
        FoodPrepared (item, tabId) |> toListOK
    else
      CanNotPrepareNonOrderedFood item |> fail

let handleServeFood item tabId = function
| OrderInProgress ipo ->
  if List.contains item ipo.PlacedOrder.Foods then
    if List.contains item ipo.ServedFoods then
      CanNotServeAlreadyServedFood item |> fail
    else
      if List.contains item ipo.PreparedFoods then
        FoodServed (item, tabId) |> toListOK
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
    TabClosed payment |> toListOK
  else
    InvalidPayment (orderAmount, payment.Amount) |> fail
| _ -> CanNotPayForNonServedOrder |> fail

let execute state command =
    match command with
    | OpenTab tab -> handleOpenTab tab state
    | PlaceOrder order -> handlePlaceOrder order state
    | ServeDrink (item, tabId) -> handleServeDrink item tabId state
    | PrepareFood (item, tabId) -> handlePrepareFood item tabId state
    | ServeFood (item, tabId) -> handleServeFood item tabId state
    | CloseTab payment -> handleCloseTab payment state

let evolve state command =
  match execute state command with
  | Ok (events,_) ->
    let newState = List.fold apply state events
    (newState, events) |> ok
  | Bad err -> Bad err