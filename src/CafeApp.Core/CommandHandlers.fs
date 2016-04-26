module CommandHandlers
open Chessie.ErrorHandling
open States
open Events
open System
open Domain
open Commands
open Errors

let toList x = [x]

let handleOpenTab tab = function
| ClosedTab _ -> [TabOpened tab] |> ok
| _ -> TabAlreadyOpened |> fail

let handlePlaceOrder order = function
| OpenedTab _ ->
  if List.isEmpty order.Foods && List.isEmpty order.Drinks then
    fail CanNotPlaceEmptyOrder
  else
    [OrderPlaced order] |> ok
| ClosedTab _ -> fail CanNotOrderWithClosedTab
| _ -> fail OrderAlreadyPlaced

let handleServeDrink drink tabId state =
  let isOrderedDrink drink order =
    List.contains drink order.Drinks
  match state with
  | PlacedOrder order ->
      if isOrderedDrink drink order then
        let event = DrinkServed (drink,tabId)
        if order.Drinks = [drink] then
          let payment = {Tab = order.Tab; Amount = orderAmount order}
          event :: [OrderServed (order, payment)] |> ok
        else
          [event] |> ok
      else
        CanNotServeNonOrderedDrink drink |> fail
  | ServedOrder _ -> OrderAlreadyServed |> fail
  | OpenedTab _ ->  CanNotServeForNonPlacedOrder |> fail
  | ClosedTab _ -> CanNotServeWithClosedTab |> fail
  | OrderInProgress ipo ->
      let order = ipo.PlacedOrder
      if isOrderedDrink drink order then
        let nonServedDrinks = nonServedDrinks ipo
        if List.contains drink nonServedDrinks then
          let event = DrinkServed (drink,tabId)
          if isServingDrinkCompletesOrder ipo drink then
            event :: [OrderServed (order, payment ipo)] |> ok
          else
            [event] |> ok
        else
          CanNotServeAlreadyServedDrink drink |> fail
      else
        CanNotServeNonOrderedDrink drink |> fail

let handlePrepareFood food tabId state =
  let isOrderedFood food order =
    List.contains food order.Foods
  match state with
  | PlacedOrder order ->
    if isOrderedFood food order then
      [FoodPrepared (food, tabId)] |> ok
    else
      CanNotPrepareNonOrderedFood food |> fail
  | ServedOrder _ -> OrderAlreadyServed |> fail
  | OpenedTab _ ->  CanNotPrepareForNonPlacedOrder |> fail
  | ClosedTab _ -> CanNotPrepareWithClosedTab |> fail
  | OrderInProgress ipo ->
    if isOrderedFood food ipo.PlacedOrder then
      if List.contains food ipo.PreparedFoods then
        CanNotPrepareAlreadyPreparedFood food |> fail
      else
        [FoodPrepared (food, tabId)] |> ok
    else
      CanNotPrepareNonOrderedFood food |> fail

let handleServeFood food tabId = function
| OrderInProgress ipo ->
  if List.contains food ipo.PlacedOrder.Foods then
    if List.contains food ipo.ServedFoods then
      CanNotServeAlreadyServedFood food |> fail
    else
      if List.contains food ipo.PreparedFoods then
        let event = FoodServed (food, tabId)
        if isServingFoodCompletesOrder ipo food then
          event :: [OrderServed (ipo.PlacedOrder, payment ipo)] |> ok
        else
          [event] |> ok
      else
        CanNotServeNonPreparedFood food |> fail
  else
    CanNotServeNonOrderedFood food |> fail
| PlacedOrder _ -> CanNotServeNonPreparedFood food |> fail
| ServedOrder _ -> OrderAlreadyServed |> fail
| OpenedTab _ -> CanNotServeForNonPlacedOrder |> fail
| ClosedTab _ -> CanNotServeWithClosedTab |> fail

let handleCloseTab payment = function
| ServedOrder order ->
  let orderAmount = orderAmount order
  if payment.Amount = orderAmount then
    [TabClosed payment] |> ok
  else
    InvalidPayment (orderAmount, payment.Amount) |> fail
| _ -> CanNotPayForNonServedOrder |> fail

let execute state command =
    match command with
    | OpenTab tab -> handleOpenTab tab state
    | PlaceOrder order -> handlePlaceOrder order state
    | ServeDrink (food, tabId) -> handleServeDrink food tabId state
    | PrepareFood (food, tabId) -> handlePrepareFood food tabId state
    | ServeFood (food, tabId) -> handleServeFood food tabId state
    | CloseTab payment -> handleCloseTab payment state

let evolve state command =
  match execute state command with
  | Ok (events,_) ->
    let newState = List.fold apply state events
    (newState, events) |> ok
  | Bad err -> Bad err