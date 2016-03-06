module Errors

type Error =
  | TabAlreadyOpened
  | CanNotOrderWithClosedTab
  | OrderAlreadyPlaced