module CommandHandlers
open States
open Events
open System
open Domain

let execute state command =
  match command with
  | _ -> TabOpened {Id = Guid.NewGuid(); TableNumber = 1}

let evolve state command =
  let event = execute state command
  let newState = apply state event
  (newState, event)