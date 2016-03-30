module QueriesApi
open Queries
open Suave
open Suave.Filters
open Suave.Operators
open JsonFormatter

let tables getTables (context : HttpContext) = async {
  let! tables = getTables ()
  return! toTablesJSON tables context
}

let queriesApi queries =
  choose [
    GET >=> path "/tables"
        >=> tables queries.GetTables
  ]