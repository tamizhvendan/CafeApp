module OpenTabTests
open NUnit.Framework
open CafeAppTestsDSL
open Domain
open Events
open Commands
open States
open System
open Errors

[<Test>]
let ``Can Open a new Tab``() =
  let tab = {Id = Guid.NewGuid(); TableNumber = 1}

  Given (ClosedTab None)
  |> When (OpenTab tab)
  |> ThenStateShouldBe (OpenedTab tab)
  |> WithEvent (TabOpened tab)

[<Test>]
let ``Cannot open an already Opened tab`` () =
  let tab = {Id = Guid.NewGuid(); TableNumber = 1}

  Given (OpenedTab tab)
  |> When (OpenTab tab)
  |> ShouldFailWith TabAlreadyOpened