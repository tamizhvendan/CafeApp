module CloseTab
open FSharp.Data
open Domain
open ReadModel
open CommandHandlers
open Commands

[<Literal>]
let CloseTabJson = """{
    "closeTab" : {
      "tabId" : "2a964d85-f503-40a1-8014-2c8ee5ac4a49",
      "amount" : 10.1
    }
}"""
type CloseTabReq = JsonProvider<CloseTabJson>
let (|CloseTabRequest|_|) payload =
  try
    let req = CloseTabReq.Parse(payload).CloseTab
    (req.TabId, req.Amount) |> Some
  with
  | ex -> None


let validateCloseTab getTableByTabId (tabId, amount) = async {
  let! table = getTableByTabId tabId
  match table with
  | Some t ->
    let tab = {Id = tabId; TableNumber = t.Number}
    return Choice1Of2 {Amount = amount; Tab = tab}
  | _ ->
    return Choice2Of2 "Invalid Tab ID"
}

let closeTabCommander getTableByTabId = {
  Validate = validateCloseTab getTableByTabId
  ToCommand = CloseTab
}