module Events
open Domain
open System

type Event =
  | TabOpened of Tab
  | OrderPlaced of Order
  | DrinkServed of Drink * Guid
  | FoodPrepared of Food * Guid
  | FoodServed of Food * Guid
  | TabClosed of Payment