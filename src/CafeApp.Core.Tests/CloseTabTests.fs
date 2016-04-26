module CloseTabTests
open Domain
open States
open Commands
open Events
open CafeAppTestsDSL
open NUnit.Framework
open TestData
open Errors

[<Test>]
let ``Can close the tab by paying full amount`` () =
  let order = {order with
                  Foods = [salad;pizza]
                  Drinks = [coke]}
  let payment = {
    Tab = tab
    Amount = 10.5m
  }

  Given (ServedOrder order)
  |> When (CloseTab payment)
  |> ThenStateShouldBe (ClosedTab (Some tab.Id))
  |> WithEvents [TabClosed payment]

[<Test>]
let ``Can not close a tab with invalid payment amount`` () =
  let order = {order with
                  Foods = [salad;pizza]
                  Drinks = [coke]}

  Given (ServedOrder order)
  |> When (CloseTab {Tab = tab; Amount = 9.5m})
  |> ShouldFailWith (InvalidPayment (10.5m, 9.5m))