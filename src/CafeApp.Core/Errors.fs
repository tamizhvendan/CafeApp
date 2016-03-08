module Errors
open Domain

type Error =
  | TabAlreadyOpened
  | CanNotPlaceEmptyOrder
  | CanNotOrderWithClosedTab
  | OrderAlreadyPlaced
  | CanNotServeNonOrderedDrinks of DrinksItem
  | OrderAlreadyServed
  | CanNotServeForNonPlacedOrder of DrinksItem
  | CanNotServeWithClosedTab