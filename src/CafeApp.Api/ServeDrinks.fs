module ServeDrinks
open FSharp.Data
open Commands
open CommandHandlers

[<Literal>]
let ServeDrinksJson = """{
    "serveDrinks" : {
      "tabId" : "2a964d85-f503-40a1-8014-2c8ee5ac4a49",
      "menuNumber" : 10
    }
}"""
type ServeDrinksReq = JsonProvider<ServeDrinksJson>

let (|ServeDrinksRequest|_|) payload =
  try
    let req = ServeDrinksReq.Parse(payload).ServeDrinks
    (req.TabId, req.MenuNumber) |> Some
  with
  | ex -> None


let validateServeDrinks getTableByTabId
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

let serveDrinksCommander getTableByTabId
  getDrinksByMenuNumber =
  let validate =
    getDrinksByMenuNumber
    |> validateServeDrinks getTableByTabId
  {
    Validate = validate
    ToCommand = ServeDrinks
  }