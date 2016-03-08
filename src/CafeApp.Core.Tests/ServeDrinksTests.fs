module ServeDrinksTests
open Domain
open States
open Commands
open Events
open CafeAppTestsDSL
open NUnit.Framework
open TestData
open Errors

[<Test>]
let ``Can Serve Drinks`` () =
  let order = {order with DrinksItems = [coke;lemonade]}
  let expected = {
      PlacedOrder = order
      ServedDrinks = [coke]
      PreparedFoods = []
      ServedFoods = []}
  Given (PlacedOrder order)
  |> When (ServeDrinks (coke, order.TabId))
  |> ThenStateShouldBe (OrderInProgress expected)
  |> WithEvent (DrinksServed (coke, order.TabId))

[<Test>]
let ``Can not serve non ordered drinks`` () =
  let order = {order with DrinksItems = [coke]}
  Given (PlacedOrder order)
  |> When (ServeDrinks (lemonade, order.TabId))
  |> ShouldFailWith (CanNotServeNonOrderedDrinks lemonade)

[<Test>]
let ``Can not serve drinks for already served order`` () =
  Given (OrderServed order)
  |> When (ServeDrinks (coke, order.TabId))
  |> ShouldFailWith OrderAlreadyServed

[<Test>]
let ``Can not serve drinks for non placed order`` () =
  Given (OpenedTab tab)
  |> When (ServeDrinks (coke, tab.Id))
  |> ShouldFailWith CanNotServeForNonPlacedOrder

[<Test>]
let ``Can not serve with closed tab`` () =
  Given (ClosedTab None)
  |> When (ServeDrinks (coke, tab.Id))
  |> ShouldFailWith (CanNotServeWithClosedTab)

[<Test>]
let ``Can complete the order by serving drinks`` () =
  let order = {order with DrinksItems = [coke;lemonade]}
  let orderInProgress = {
    PlacedOrder = order
    ServedDrinks = [coke]
    PreparedFoods = []
    ServedFoods = []
  }
  Given (OrderInProgress orderInProgress)
  |> When (ServeDrinks (lemonade, order.TabId))
  |> ThenStateShouldBe (OrderServed order)
  |> WithEvent (DrinksServed (lemonade, order.TabId))

[<Test>]
let ``Can maintain the order in progress state by serving drinks`` () =
  let order = {order with DrinksItems = [coke;lemonade;appleJuice]}
  let orderInProgress = {
    PlacedOrder = order
    ServedDrinks = [coke]
    PreparedFoods = []
    ServedFoods = []
  }
  let expected =
    {orderInProgress with
        ServedDrinks = lemonade :: orderInProgress.ServedDrinks}

  Given (OrderInProgress orderInProgress)
  |> When (ServeDrinks (lemonade, order.TabId))
  |> ThenStateShouldBe (OrderInProgress expected)
  |> WithEvent (DrinksServed (lemonade, order.TabId))

[<Test>]
let ``Can serve drinks for order containing only one drinks`` () =
  let order = {order with DrinksItems = [coke]}
  Given (PlacedOrder order)
  |> When (ServeDrinks (coke, order.TabId))
  |> ThenStateShouldBe (OrderServed order)
  |> WithEvent (DrinksServed (coke, order.TabId))