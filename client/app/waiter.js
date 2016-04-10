import {FoodServed} from './events.js';
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
  if (action.type === FoodServed) {
    let waiterToDos = state.waiterToDos.map(waiterToDo => {
      if (waiterToDo.tabId === action.data.tabId) {
        let foodItems = waiterToDo.foodItems.filter(foodItem =>
                            foodItem.menuNumber !== action.data.food.menuNumber)
        return update(waiterToDo, {foodItems : {$set : foodItems}})
      }
      return waiterToDo;
    })
    return {waiterToDos}
  }
  return state;
}