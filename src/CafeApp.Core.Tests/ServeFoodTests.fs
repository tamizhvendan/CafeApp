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
  let order = {order with FoodItems = [salad]}
  let orderInProgress = {
    PlacedOrder = order
    ServedFoods = []
    ServedDrinks = []
    PreparedFoods = [salad]
  }

  Given (OrderInProgress orderInProgress)
  |> When (ServeFood (salad, order.TabId))
  |> ThenStateShouldBe (OrderServed order)
  |> WithEvent (FoodServed (salad, order.TabId))

[<Test>]
let ``Can maintain the order in progress state by serving food`` () =
  let order = {order with FoodItems = [salad;pizza]}
  let orderInProgress = {
    PlacedOrder = order
    ServedFoods = []
    ServedDrinks = []
    PreparedFoods = [salad;pizza]
  }
  let expected = {orderInProgress with ServedFoods = [salad]}

  Given (OrderInProgress orderInProgress)
  |> When (ServeFood (salad, order.TabId))
  |> ThenStateShouldBe (OrderInProgress expected)
  |> WithEvent (FoodServed (salad, order.TabId))

[<Test>]
let ``Can serve only prepared food`` () =
  let order = {order with FoodItems = [salad;pizza]}
  let orderInProgress = {
    PlacedOrder = order
    ServedFoods = []
    ServedDrinks = []
    PreparedFoods = [salad]
  }

  Given (OrderInProgress orderInProgress)
  |> When (ServeFood (pizza, order.TabId))
  |> ShouldFailWith (CanNotServeNonPreparedFood pizza)

[<Test>]
let ``Can not serve non-ordered food`` () =
  let order = {order with FoodItems = [salad;]}
  let orderInProgress = {
    PlacedOrder = order
    ServedFoods = []
    ServedDrinks = []
    PreparedFoods = [salad]
  }

  Given (OrderInProgress orderInProgress)
  |> When (ServeFood (pizza, order.TabId))
  |> ShouldFailWith (CanNotServeNonOrderedFood pizza)

[<Test>]
let ``Can not serve already served food`` () =
  let order = {order with FoodItems = [salad;pizza]}
  let orderInProgress = {
    PlacedOrder = order
    ServedFoods = [salad]
    ServedDrinks = []
    PreparedFoods = [pizza]
  }

  Given (OrderInProgress orderInProgress)
  |> When (ServeFood (salad, order.TabId))
  |> ShouldFailWith (CanNotServeAlreadyServedFood salad)

[<Test>]
let ``Can not serve for placed order`` () =
  Given (PlacedOrder order)
  |> When (ServeFood (salad, order.TabId))
  |> ShouldFailWith (CanNotServeNonPreparedFood salad)

[<Test>]
let ``Can not serve for non placed order`` () =
  Given (OpenedTab tab)
  |> When (ServeFood (salad, order.TabId))
  |> ShouldFailWith CanNotServeForNonPlacedOrder

[<Test>]
let ``Can not serve for already served order`` () =
  Given (OrderServed order)
  |> When (ServeFood (salad, order.TabId))
  |> ShouldFailWith OrderAlreadyServed

[<Test>]
let ``Can not serve with closed tab`` () =
  Given (ClosedTab None)
  |> When (ServeFood (salad, order.TabId))
  |> ShouldFailWith CanNotServeWithClosedTab