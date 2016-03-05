module States
open Domain
open System

type State =
  | ClosedTab of Guid option
  | OpenedTab of Tab
  | PlacedOrder of Order
  | OrderInProgress of InProgressOrder
  | OrderServed of Order

let apply state event =
  match state,event with
  | _ -> ClosedTab None