module ServeDrink
open FSharp.Data
open Commands
open CommandHandlers

[<Literal>]
let ServeDrinkJson = """{
    "serveDrink" : {
      "tabId" : "2a964d85-f503-40a1-8014-2c8ee5ac4a49",
      "menuNumber" : 10
    }
}"""
type ServeDrinkReq = JsonProvider<ServeDrinkJson>

let (|ServeDrinkRequest|_|) payload =
  try
    let req = ServeDrinkReq.Parse(payload).ServeDrink
    (req.TabId, req.MenuNumber) |> Some
  with
  | ex -> None


let validateServeDrink getTableByTabId
  getDrinksByMenuNumber (tabId, drinksMenuNumber) = async {
    let! table = getTableByTabId tabId
    match table with
    | Some _ ->
      let! drinks = getDrinksByMenuNumber drinksMenuNumber
      match drinks with
      | Some d ->
        return Choice1Of2 (d, tabId)
      | _ -> return Choice2Of2 "Invalid Drinks Menu Number"
    | _ -> return Choice2Of2 "Invalid Tab Id"
}

let serveDrinkCommander getTableByTabId
  getDrinksByMenuNumber =
  let validate =
    getDrinksByMenuNumber
    |> validateServeDrink getTableByTabId
  {
    Validate = validate
    ToCommand = ServeDrink
  }