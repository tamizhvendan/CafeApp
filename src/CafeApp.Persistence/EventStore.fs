module EventStore
open States
open System
open NEventStore
open Events

let getTabIdFromState = function
| ClosedTab None -> None
| OpenedTab tab -> Some tab.Id
| PlacedOrder po -> Some po.Tab.Id
| OrderInProgress ipo -> Some ipo.PlacedOrder.Tab.Id
| ServedOrder payment -> Some payment.Tab.Id
| ClosedTab (Some tabId) -> Some tabId

let saveEvent (storeEvents : IStoreEvents) state event  =
  match getTabIdFromState state with
  | Some tabId ->
    use stream = storeEvents.OpenStream(tabId.ToString())
    stream.Add(new EventMessage(Body = event))
    stream.CommitChanges(Guid.NewGuid())
  | _ -> ()

let saveEvents (storeEvents : IStoreEvents) state events =
  events
  |> List.iter (saveEvent storeEvents state)
  |> async.Return

let getEvents (storeEvents : IStoreEvents) (tabId : Guid) =
  use stream = storeEvents.OpenStream(tabId.ToString())
  stream.CommittedEvents
  |> Seq.map (fun msg -> msg.Body)
  |> Seq.cast<Event>

let getState storeEvents tabId =
  getEvents storeEvents tabId
  |> Seq.fold apply (ClosedTab None)
  |> async.Return

type EventStore = {
  GetState : Guid -> Async<State>
  SaveEvents : State -> Event list -> Async<unit>
}