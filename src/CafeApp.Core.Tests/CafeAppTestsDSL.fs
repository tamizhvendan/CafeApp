module CafeAppTestsDSL
open FsUnit
open CommandHandlers

let Given = id
let When command state = (command, state)
let ThenStateShouldBe expectedState (command, state) =
  let actualState,event = evolve state command
  actualState |> should equal expectedState
  event
let WithEvent expectedEvent actualEvent =
  actualEvent |> should equal expectedEvent