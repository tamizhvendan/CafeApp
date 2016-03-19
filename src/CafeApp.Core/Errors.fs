module Errors
open Domain

type Error =
  | TabAlreadyOpened
  | CanNotPlaceEmptyOrder
  | CanNotOrderWithClosedTab
  | OrderAlreadyPlaced
  | CanNotServeNonOrderedDrinks of DrinksItem
  | CanNotServeAlreadyServedDrinks of DrinksItem
  | OrderAlreadyServed
  | CanNotServeForNonPlacedOrder
  | CanNotServeWithClosedTab
  | CanNotPrepareNonOrderedFood of FoodItem
  | CanNotPrepareAlreadyPreparedFood of FoodItem
  | CanNotServeNonPreparedFood of FoodItem
  | CanNotServeNonOrderedFood of FoodItem
  | CanNotServeAlreadyServedFood of FoodItem
  | CanNotPrepareForNonPlacedOrder
  | CanNotPrepareWithClosedTab
  | InvalidPayment of decimal * decimal
  | CanNotPayForNonServedOrder

let toErrorString = function
| TabAlreadyOpened -> "Tab Already Opened"
| CanNotOrderWithClosedTab -> "Cannot Order as Tab is Closed"
| OrderAlreadyPlaced -> "Order already placed"
| CanNotServeNonOrderedDrinks (DrinksItem item)  ->
  sprintf "DrinksItem %s(%d) is not ordered" item.Name item.MenuNumber
| CanNotServeAlreadyServedDrinks (DrinksItem item)  ->
  sprintf "DrinksItem %s(%d) is already served" item.Name item.MenuNumber
| CanNotServeNonOrderedFood (FoodItem item) ->
  sprintf "FoodItem %s(%d) is not ordered" item.Name item.MenuNumber
| CanNotPrepareNonOrderedFood (FoodItem item) ->
  sprintf "FoodItem %s(%d) is not ordered" item.Name item.MenuNumber
| CanNotServeNonPreparedFood (FoodItem item) ->
  sprintf "FoodItem %s(%d) is not prepared yet" item.Name item.MenuNumber
| CanNotPrepareAlreadyPreparedFood (FoodItem item) ->
  sprintf "FoodItem %s(%d) is already prepared" item.Name item.MenuNumber
| CanNotServeAlreadyServedFood (FoodItem item) ->
  sprintf "FoodItem %s(%d) is already served" item.Name item.MenuNumber
| CanNotServeWithClosedTab -> "Cannot Serve as Tab is Closed"
| CanNotPrepareWithClosedTab -> "Cannot Prepare as Tab is Closed"
| OrderAlreadyServed -> "Order Already Served"
| InvalidPayment (expected, actual) ->
  sprintf "Invalid Payment. Expected is %f but paid %f" expected actual
| CanNotPayForNonServedOrder -> "Can not pay for non served order"
| CanNotPlaceEmptyOrder -> "Can not place empty order"
| CanNotPrepareForNonPlacedOrder ->
  "Can not prepare for non placed order"
| CanNotServeForNonPlacedOrder -> "Can not serve for non placed order"