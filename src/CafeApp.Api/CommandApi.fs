module CommandApi

open System.Text
open CommandHandlers
open OpenTab
open Queries
open Chessie.ErrorHandling

let handleCommandRequest validationQueries eventStore
  = function
  | OpenTabRequest tab ->
      validationQueries.GetTableByTableNumber
      |> openTabCommander
      |> handleCommand eventStore tab
  | _ -> err "Invalid command" |> fail |> async.Return