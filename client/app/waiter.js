import {FoodServed, DrinksServed, OrderPlaced, FoodPrepared} from './events.js';
import update from 'react-addons-update';

const intialWaiterTodosState = {
  waiterToDos : []
}

const WaiterToDoListSuccess = "WAITER_TODO_LIST_SUCCESS"

export function listWaiterToDos(waiterToDos) {
  return {
    type : WaiterToDoListSuccess,
    waiterToDos : waiterToDos
  }
}

export function waiterToDosReducer(state = intialWaiterTodosState, action) {
  if (action.type === WaiterToDoListSuccess) {
    return action.waiterToDos;
  }
  if (action.type === OrderPlaced) {
    let order = action.data
    let todo = {
      tabId : order.tabId,
      tableNumber : order.tableNumber,
      drinksItems : order.drinksItems,
      foodItems : []
    }
    return {waiterToDos : state.waiterToDos.concat(todo)}
  }
  if (action.type === FoodPrepared) {
    let waiterToDos = state.waiterToDos.map(waiterToDo => {
      if (waiterToDo.tabId === action.data.tabId) {
        let foodItems = waiterToDo.foodItems.concat(action.data.food);
        return update(waiterToDo, {foodItems : {$set : foodItems}})
      }
      return waiterToDo;
    })
    return {waiterToDos}
  }
  if (action.type === FoodServed) {
    let waiterToDos = state.waiterToDos.map(waiterToDo => {
      if (waiterToDo.tabId === action.data.tabId) {
        let foodItems = waiterToDo.foodItems.filter(item =>
                            item.menuNumber !== action.data.food.menuNumber)
        return update(waiterToDo, {foodItems : {$set : foodItems}})
      }
      return waiterToDo;
    })
    return {waiterToDos}
  }
  if (action.type === DrinksServed) {
    let waiterToDos = state.waiterToDos.map(waiterToDo => {
      if (waiterToDo.tabId === action.data.tabId) {
        let drinksItems = waiterToDo.drinksItems.filter(item =>
                            item.menuNumber !== action.data.drinks.menuNumber)
        return update(waiterToDo, {drinksItems : {$set : drinksItems}})
      }
      return waiterToDo;
    })
    return {waiterToDos}
  }
  return state;
}