module Commands
open Domain
open System

type Command =
  | OpenTab of Tab
  | PlaceOrder of Order
  | ServeDrink of Drink * Guid
  | PrepareFood of Food * Guid
  | ServeFood of Food * Guid
  | CloseTab of Payment