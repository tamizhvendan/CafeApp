module Errors
open Domain

type Error =
  | TabAlreadyOpened
  | CanNotPlaceEmptyOrder
  | CanNotOrderWithClosedTab
  | OrderAlreadyPlaced
  | CanNotServeNonOrderedDrink of Drink
  | CanNotServeAlreadyServedDrink of Drink
  | OrderAlreadyServed
  | CanNotServeForNonPlacedOrder
  | CanNotServeWithClosedTab
  | CanNotPrepareNonOrderedFood of Food
  | CanNotPrepareAlreadyPreparedFood of Food
  | CanNotServeNonPreparedFood of Food
  | CanNotServeNonOrderedFood of Food
  | CanNotServeAlreadyServedFood of Food
  | CanNotPrepareForNonPlacedOrder
  | CanNotPrepareWithClosedTab
  | InvalidPayment of decimal * decimal
  | CanNotPayForNonServedOrder

let toErrorString = function
| TabAlreadyOpened -> "Tab Already Opened"
| CanNotOrderWithClosedTab -> "Cannot Order as Tab is Closed"
| OrderAlreadyPlaced -> "Order already placed"
| CanNotServeNonOrderedDrink (Drink item)  ->
  sprintf "Drink %s(%d) is not ordered" item.Name item.MenuNumber
| CanNotServeAlreadyServedDrink (Drink item)  ->
  sprintf "Drink %s(%d) is already served" item.Name item.MenuNumber
| CanNotServeNonOrderedFood (Food item) ->
  sprintf "Food %s(%d) is not ordered" item.Name item.MenuNumber
| CanNotPrepareNonOrderedFood (Food item) ->
  sprintf "Food %s(%d) is not ordered" item.Name item.MenuNumber
| CanNotServeNonPreparedFood (Food item) ->
  sprintf "Food %s(%d) is not prepared yet" item.Name item.MenuNumber
| CanNotPrepareAlreadyPreparedFood (Food item) ->
  sprintf "Food %s(%d) is already prepared" item.Name item.MenuNumber
| CanNotServeAlreadyServedFood (Food item) ->
  sprintf "Food %s(%d) is already served" item.Name item.MenuNumber
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