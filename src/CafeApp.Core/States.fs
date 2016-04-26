module States
open Domain
open System
open Events

type State =
  | ClosedTab of Guid option
  | OpenedTab of Tab
  | PlacedOrder of Order
  | OrderInProgress of InProgressOrder
  | ServedOrder of Order

let getState (ipo : InProgressOrder) =
  if isServedOrder ipo then
    ServedOrder ipo.PlacedOrder
  else
    OrderInProgress ipo

let apply state event =
  match state,event with
  | ClosedTab _, TabOpened tab -> OpenedTab tab
  | OpenedTab _, OrderPlaced order -> PlacedOrder order
  | PlacedOrder order, DrinkServed (item,_) ->
    {
      PlacedOrder = order
      ServedDrinks = [item]
      ServedFoods = []
      PreparedFoods = []
    } |> getState
  | OrderInProgress ipo, DrinkServed (item,_) ->
    {ipo with ServedDrinks = item :: ipo.ServedDrinks}
    |> getState
  | PlacedOrder order, FoodPrepared (item,_) ->
    {
      PlacedOrder = order
      PreparedFoods = [item]
      ServedDrinks = []
      ServedFoods = []
    } |> OrderInProgress
  | OrderInProgress ipo, FoodPrepared (item, _) ->
    {ipo with PreparedFoods = item :: ipo.PreparedFoods}
    |> OrderInProgress
  | OrderInProgress ipo, FoodServed (item, _) ->
    {ipo with ServedFoods = item :: ipo.ServedFoods}
    |> getState
  | ServedOrder order, TabClosed payment ->
    ClosedTab (Some payment.Tab.Id)
  | _ -> state