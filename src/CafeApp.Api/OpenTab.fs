module OpenTab
open Domain
open System
open FSharp.Data
open Commands
open Queries
open CommandHandlers

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

let validateOpenTab isValidTableNumber tab = async {
  let! isValid = isValidTableNumber tab.TableNumber
  if isValid then
    return Choice1Of2 tab
  else
    return Choice2Of2 "Invalid Table Number"
}

let openTabCommander isValidTableNumber = {
  Validate = validateOpenTab isValidTableNumber
  ToCommand = OpenTab
}