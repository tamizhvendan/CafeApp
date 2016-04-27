module ServeFood
open FSharp.Data
open CommandHandlers
open PrepareFood
open Commands

[<Literal>]
let ServeFoodJson = """{
    "serveFood" : {
      "tabId" : "2a964d85-f503-40a1-8014-2c8ee5ac4a49",
      "menuNumber" : 10
    }
}"""
type ServeFoodReq = JsonProvider<ServeFoodJson>
let (|ServeFoodRequest|_|) payload =
  try
    let req = ServeFoodReq.Parse(payload).ServeFood
    (req.TabId, req.MenuNumber) |> Some
  with
  | ex -> None

let serveFoodCommander getTableByTabId
  getFoodByMenuNumber = {
    Validate =
      validateFood getTableByTabId getFoodByMenuNumber
    ToCommand = ServeFood
  }