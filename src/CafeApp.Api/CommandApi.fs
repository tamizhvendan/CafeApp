module CommandApi

open System.Text
open CommandHandlers
open OpenTab
open Queries

let handleCommandRequest validationQueries eventStore
  = function
  | OpenTabRequest tab ->
      validationQueries.GetTableByTableNumber
      |> openTabCommander
      |> handleCommand eventStore tab
  | _ -> failwith "TODO"