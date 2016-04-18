import {FoodServed, DrinkServed, OrderPlaced, FoodPrepared, TabClosed} from './events.js';
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
    let order = action.data.order
    let todo = {
      tabId : order.tabId,
      tableNumber : order.tableNumber,
      drinks : order.drinks,
      foods : []
    }
    return {waiterToDos : state.waiterToDos.concat(todo)}
  }
  if (action.type === FoodPrepared) {
    let waiterToDos = state.waiterToDos.map(waiterToDo => {
      if (waiterToDo.tabId === action.data.tabId) {
        let foods = waiterToDo.foods.concat(action.data.food);
        return update(waiterToDo, {foods : {$set : foods}})
      }
      return waiterToDo;
    })
    return {waiterToDos}
  }
  if (action.type === FoodServed) {
    let waiterToDos = state.waiterToDos.map(waiterToDo => {
      if (waiterToDo.tabId === action.data.tabId) {
        let foods = waiterToDo.foods.filter(food =>
                            food.menuNumber !== action.data.food.menuNumber)
        return update(waiterToDo, {foods : {$set : foods}})
      }
      return waiterToDo;
    })
    return {waiterToDos}
  }
  if (action.type === DrinkServed) {
    let waiterToDos = state.waiterToDos.map(waiterToDo => {
      if (waiterToDo.tabId === action.data.tabId) {
        let drinks = waiterToDo.drinks.filter(drink =>
                            drink.menuNumber !== action.data.drink.menuNumber)
        return update(waiterToDo, {drinks : {$set : drinks}})
      }
      return waiterToDo;
    })
    return {waiterToDos}
  }

  if (action.type === TabClosed){
    let waiterToDos = state.waiterToDos.filter(todo => todo.tabId !== action.data.tabId)
    return {waiterToDos}
  }

  return state;
}