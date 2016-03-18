module OpenTab
open Domain
open System
open FSharp.Data
open Commands
open Queries

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

let validateOpenTab validationQueries tab = async {
  let! isValid =
    validationQueries.IsValidTableNumber tab.TableNumber
  if isValid then
    return Choice1Of2 tab
  else
    return Choice2Of2 "Invalid Table Number"
}

let toOpenTabCommand = OpenTab