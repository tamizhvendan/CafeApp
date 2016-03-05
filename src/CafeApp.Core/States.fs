module States
open Domain
open System
open Events

type State =
  | ClosedTab of Guid option
  | OpenedTab of Tab
  | PlacedOrder of Order
  | OrderInProgress of InProgressOrder
  | OrderServed of Order

let apply state event =
  match state,event with
  | ClosedTab _, TabOpened tab -> OpenedTab tab
  | _ -> state