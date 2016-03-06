module CafeAppTestsDSL
open FsUnit
open CommandHandlers
open Chessie.ErrorHandling
open NUnit.Framework
open Errors
open States

let Given (state : State) = state
let When command state = (command, state)
let ThenStateShouldBe expectedState (command, state) =
  match evolve state command with
  | Ok((actualState,event),_) ->
      actualState |> should equal expectedState
      event |> Some
  | Bad errs ->
      sprintf "Expected : %A, But Actual : %A" expectedState errs.Head
      |> Assert.Fail
      None
let WithEvent expectedEvent actualEvent =
  match actualEvent with
  | Some (actualEvent) ->
    actualEvent |> should equal expectedEvent
  | None -> None |> should equal expectedEvent

let ShouldFailWith (expectedError : Error) (command, state) =
  match evolve state command with
  | Bad errs -> errs.Head |> should equal expectedError
  | Ok(r,_) ->
      sprintf "Expected : %A, But Actual : %A" expectedError r
      |> Assert.Fail