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