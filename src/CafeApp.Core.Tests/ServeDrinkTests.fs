module ServeDrinkTests
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
  let order = {order with Drinks = [coke;lemonade]}
  let expected = {
      PlacedOrder = order
      ServedDrinks = [coke]
      PreparedFoods = []
      ServedFoods = []}
  Given (PlacedOrder order)
  |> When (ServeDrink (coke, order.Tab.Id))
  |> ThenStateShouldBe (OrderInProgress expected)
  |> WithEvents [DrinkServed (coke, order.Tab.Id)]

[<Test>]
let ``Can not serve non ordered drinks`` () =
  let order = {order with Drinks = [coke]}
  Given (PlacedOrder order)
  |> When (ServeDrink (lemonade, order.Tab.Id))
  |> ShouldFailWith (CanNotServeNonOrderedDrink lemonade)

[<Test>]
let ``Can not serve drinks for already served order`` () =
  Given (ServedOrder order)
  |> When (ServeDrink (coke, order.Tab.Id))
  |> ShouldFailWith OrderAlreadyServed

[<Test>]
let ``Can not serve drinks for non placed order`` () =
  Given (OpenedTab tab)
  |> When (ServeDrink (coke, tab.Id))
  |> ShouldFailWith CanNotServeForNonPlacedOrder

[<Test>]
let ``Can not serve with closed tab`` () =
  Given (ClosedTab None)
  |> When (ServeDrink (coke, tab.Id))
  |> ShouldFailWith (CanNotServeWithClosedTab)

[<Test>]
let ``Can complete the order by serving drinks`` () =
  let order = {order with Drinks = [coke;lemonade]}
  let orderInProgress = {
    PlacedOrder = order
    ServedDrinks = [coke]
    PreparedFoods = []
    ServedFoods = []
  }
  let amount = drinkPrice coke + drinkPrice lemonade
  let payment = { Tab = tab; Amount = amount}
  Given (OrderInProgress orderInProgress)
  |> When (ServeDrink (lemonade, order.Tab.Id))
  |> ThenStateShouldBe (ServedOrder order)
  |> WithEvents [
      DrinkServed (lemonade, order.Tab.Id)
      OrderServed (order, payment)
    ]

[<Test>]
let ``Can maintain the order in progress state by serving drinks`` () =
  let order = {order with Drinks = [coke;lemonade;appleJuice]}
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
  |> When (ServeDrink (lemonade, order.Tab.Id))
  |> ThenStateShouldBe (OrderInProgress expected)
  |> WithEvents [DrinkServed (lemonade, order.Tab.Id)]

[<Test>]
let ``Can serve drinks for order containing only one drinks`` () =
  let order = {order with Drinks = [coke]}
  let payment = {Tab = order.Tab; Amount = drinkPrice coke}

  Given (PlacedOrder order)
  |> When (ServeDrink (coke, order.Tab.Id))
  |> ThenStateShouldBe (ServedOrder order)
  |> WithEvents [
      DrinkServed (coke, order.Tab.Id)
      OrderServed (order, payment)
    ]


[<Test>]
let ``Can not serve non ordered drinks during order in progress `` () =
  let order = {order with Drinks = [coke;lemonade]}
  let orderInProgress = {
    PlacedOrder = order
    ServedDrinks = [coke]
    PreparedFoods = []
    ServedFoods = []
  }

  Given (OrderInProgress orderInProgress)
  |> When (ServeDrink (appleJuice,order.Tab.Id))
  |> ShouldFailWith (CanNotServeNonOrderedDrink appleJuice)


[<Test>]
let ``Can not serve an already served drinks`` () =
  let order = {order with Drinks = [coke;lemonade]}
  let orderInProgress = {
    PlacedOrder = order
    ServedDrinks = [coke]
    PreparedFoods = []
    ServedFoods = []
  }

  Given (OrderInProgress orderInProgress)
  |> When (ServeDrink (coke,order.Tab.Id))
  |> ShouldFailWith (CanNotServeAlreadyServedDrink coke)