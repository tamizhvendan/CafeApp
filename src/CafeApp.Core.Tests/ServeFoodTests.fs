module ServeFoodTests

open Domain
open States
open Commands
open Events
open CafeAppTestsDSL
open NUnit.Framework
open TestData
open Errors

[<Test>]
let ``Can Complete the order by serving food`` () =
  let order = {order with Foods = [salad]}
  let orderInProgress = {
    PlacedOrder = order
    ServedFoods = []
    ServedDrinks = []
    PreparedFoods = [salad]
  }

  Given (OrderInProgress orderInProgress)
  |> When (ServeFood (salad, order.Tab.Id))
  |> ThenStateShouldBe (ServedOrder order)
  |> WithEvents [FoodServed (salad, order.Tab.Id)]

[<Test>]
let ``Can maintain the order in progress state by serving food`` () =
  let order = {order with Foods = [salad;pizza]}
  let orderInProgress = {
    PlacedOrder = order
    ServedFoods = []
    ServedDrinks = []
    PreparedFoods = [salad;pizza]
  }
  let expected = {orderInProgress with ServedFoods = [salad]}

  Given (OrderInProgress orderInProgress)
  |> When (ServeFood (salad, order.Tab.Id))
  |> ThenStateShouldBe (OrderInProgress expected)
  |> WithEvents [FoodServed (salad, order.Tab.Id)]

[<Test>]
let ``Can serve only prepared food`` () =
  let order = {order with Foods = [salad;pizza]}
  let orderInProgress = {
    PlacedOrder = order
    ServedFoods = []
    ServedDrinks = []
    PreparedFoods = [salad]
  }

  Given (OrderInProgress orderInProgress)
  |> When (ServeFood (pizza, order.Tab.Id))
  |> ShouldFailWith (CanNotServeNonPreparedFood pizza)

[<Test>]
let ``Can not serve non-ordered food`` () =
  let order = {order with Foods = [salad;]}
  let orderInProgress = {
    PlacedOrder = order
    ServedFoods = []
    ServedDrinks = []
    PreparedFoods = [salad]
  }

  Given (OrderInProgress orderInProgress)
  |> When (ServeFood (pizza, order.Tab.Id))
  |> ShouldFailWith (CanNotServeNonOrderedFood pizza)

[<Test>]
let ``Can not serve already served food`` () =
  let order = {order with Foods = [salad;pizza]}
  let orderInProgress = {
    PlacedOrder = order
    ServedFoods = [salad]
    ServedDrinks = []
    PreparedFoods = [pizza]
  }

  Given (OrderInProgress orderInProgress)
  |> When (ServeFood (salad, order.Tab.Id))
  |> ShouldFailWith (CanNotServeAlreadyServedFood salad)

[<Test>]
let ``Can not serve for placed order`` () =
  Given (PlacedOrder order)
  |> When (ServeFood (salad, order.Tab.Id))
  |> ShouldFailWith (CanNotServeNonPreparedFood salad)

[<Test>]
let ``Can not serve for non placed order`` () =
  Given (OpenedTab tab)
  |> When (ServeFood (salad, order.Tab.Id))
  |> ShouldFailWith CanNotServeForNonPlacedOrder

[<Test>]
let ``Can not serve for already served order`` () =
  Given (ServedOrder order)
  |> When (ServeFood (salad, order.Tab.Id))
  |> ShouldFailWith OrderAlreadyServed

[<Test>]
let ``Can not serve with closed tab`` () =
  Given (ClosedTab None)
  |> When (ServeFood (salad, order.Tab.Id))
  |> ShouldFailWith CanNotServeWithClosedTab