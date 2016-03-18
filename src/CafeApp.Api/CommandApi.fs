module CommandApiHandlers

open System.Text
open CommandHandlers
open OpenTab
open Queries

let handleCommandRequest validationQueries eventStore jsonPayload = function
| OpenTabRequest tab ->
  validationQueries.IsValidTableNumber
  |> openTabCommander
  |> handleCommand eventStore
| _ -> failwith "TODO"