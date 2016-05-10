module CommandHandlers
open Chessie.ErrorHandling
open States
open Events
open System
open Domain
open Commands
open Errors

let (|NonOrderedDrink|_|) order drink =
  match List.contains drink order.Drinks with
  | false -> Some drink
  | true -> None

let (|NonOrderedFood|_|) order food =
  match List.contains food order.Foods with
  | false -> Some food
  | true -> None

let (|UnPreparedFood|_|) ipo food =
  match List.contains food ipo.PreparedFoods with
  | false -> Some food
  | true -> None

let (|AlreadyServedDrink|_|) ipo drink =
  match List.contains drink ipo.ServedDrinks with
  | true -> Some drink
  | false -> None

let (|AlreadyServedFood|_|) ipo food =
  match List.contains food ipo.ServedFoods with
  | true -> Some food
  | false -> None

let (|AlreadyPreparedFood|_|) ipo food =
  match List.contains food ipo.PreparedFoods with
  | true -> Some food
  | false -> None

let (|ServeDrinkCompletesIPOrder|_|) ipo drink =
  match isServingDrinkCompletesIPOrder ipo drink with
  | true -> Some drink
  | false -> None

let (|ServeFoodCompletesIPOrder|_|) ipo food =
  match isServingFoodCompletesIPOrder ipo food with
  | true -> Some food
  | false -> None

let (|ServeDrinkCompletesOrder|_|) order drink =
  match isServingDrinkCompletesOrder order drink with
  | true -> Some drink
  | false -> None

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
    let event = DrinkServed (drink,tabId)
    match drink with
    | NonOrderedDrink order _ ->
      CanNotServeNonOrderedDrink drink |> fail
    | ServeDrinkCompletesOrder order _ ->
      let payment = {Tab = order.Tab; Amount = orderAmount order}
      event :: [OrderServed (order, payment)] |> ok
    | _ -> [event] |> ok
  | ServedOrder _ -> OrderAlreadyServed |> fail
  | OpenedTab _ ->  CanNotServeForNonPlacedOrder |> fail
  | ClosedTab _ -> CanNotServeWithClosedTab |> fail
  | OrderInProgress ipo ->
    let order = ipo.PlacedOrder
    let drinkServed = DrinkServed (drink, order.Tab.Id)
    match drink with
    | NonOrderedDrink order _ ->
      CanNotServeNonOrderedDrink drink |> fail
    | AlreadyServedDrink ipo _ ->
      CanNotServeAlreadyServedDrink drink |> fail
    | ServeDrinkCompletesIPOrder ipo _ ->
      drinkServed ::
        [OrderServed (order, payment order)]
      |> ok
    | _ -> [drinkServed] |> ok

let handlePrepareFood food tabId state =
  match state with
  | PlacedOrder order ->
    match food with
    | NonOrderedFood order _ ->
      CanNotPrepareNonOrderedFood food |> fail
    | _ -> [FoodPrepared (food, tabId)] |> ok
  | ServedOrder _ -> OrderAlreadyServed |> fail
  | OpenedTab _ ->  CanNotPrepareForNonPlacedOrder |> fail
  | ClosedTab _ -> CanNotPrepareWithClosedTab |> fail
  | OrderInProgress ipo ->
    let order = ipo.PlacedOrder
    match food with
    | NonOrderedFood order _ ->
      CanNotPrepareNonOrderedFood food |> fail
    | AlreadyPreparedFood ipo _ ->
        CanNotPrepareAlreadyPreparedFood food |> fail
    | _ -> [FoodPrepared (food, tabId)] |> ok

let handleServeFood food tabId = function
| OrderInProgress ipo ->
  let order = ipo.PlacedOrder
  let foodServed = FoodServed (food, tabId)
  match food with
  | NonOrderedFood order _ ->
    CanNotServeNonOrderedFood food |> fail
  | AlreadyServedFood ipo _ ->
    CanNotServeAlreadyServedFood food |> fail
  | UnPreparedFood ipo _ ->
    CanNotServeNonPreparedFood food |> fail
  | ServeFoodCompletesIPOrder ipo _ ->
    foodServed ::
      [OrderServed (ipo.PlacedOrder, payment order)]
    |> ok
  | _ -> [foodServed] |> ok
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