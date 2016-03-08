module OpenTabTests
open NUnit.Framework
open CafeAppTestsDSL
open Events
open Commands
open States
open System
open Errors
open TestData

[<Test>]
let ``Can Open a new Tab``() =
  Given (ClosedTab None)
  |> When (OpenTab tab)
  |> ThenStateShouldBe (OpenedTab tab)
  |> WithEvent (TabOpened tab)

[<Test>]
let ``Cannot open an already Opened tab`` () =
  Given (OpenedTab tab)
  |> When (OpenTab tab)
  |> ShouldFailWith TabAlreadyOpened