module OpenTab
open Domain
open System
open FSharp.Data
open Commands
open Queries
open CommandHandlers
open ReadModel

[<Literal>]
let OpenTabJson = """{
  "openTab" : {
    "tableNumber" : 1
  }
}"""
type OpenTabReq = JsonProvider<OpenTabJson>


let (|OpenTabRequest|_|) payload =
  try
    let req = OpenTabReq.Parse(payload).OpenTab
    { Id = Guid.NewGuid(); TableNumber = req.TableNumber}
    |> Some
  with
  | ex -> None

let validateOpenTab getTableByTableNumber tab = async {
  let! result = getTableByTableNumber tab.TableNumber
  match result with
  | Some table ->
    return Choice1Of2 tab
  | _ ->
    return Choice2Of2 "Invalid Table Number"
}

let openTabCommander getTableByTableNumber = {
  Validate = validateOpenTab getTableByTableNumber
  ToCommand = OpenTab
}