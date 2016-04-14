import {OrderPlaced, FoodPrepared, TabClosed} from './events.js';
import update from 'react-addons-update';

const intialChefTodosState = {
  chefToDos : []
}

const ChefToDoListSuccess = "CHEF_TODO_LIST_SUCCESS"

export function listChefToDos(chefToDos) {
  return {
    type : ChefToDoListSuccess,
    chefToDos : chefToDos
  }
}

export function chefToDosReducer (state = intialChefTodosState, action) {
  if (action.type === ChefToDoListSuccess) {
    return action.chefToDos;
  }
  if (action.type === OrderPlaced) {
    let order = action.data
    let chefToDo = {
      tabId : order.tabId,
      tableNumber : order.tableNumber,
      foodItems : order.foodItems
    }
    return {chefToDos : state.chefToDos.concat(chefToDo)}
  }
  if (action.type === FoodPrepared) {
    let chefToDos = state.chefToDos.map(chefToDo => {
      if (chefToDo.tabId === action.data.tabId) {
        let foodItems = chefToDo.foodItems.filter(foodItem =>
                            foodItem.menuNumber !== action.data.food.menuNumber)
        return update(chefToDo, {foodItems : {$set : foodItems}})
      }
      return chefToDo;
    })
    return {chefToDos}
  }
  if (action.type === TabClosed){
    let chefToDos = state.chefToDos.filter(todo => todo.tabId !== action.data.tabId)
    return {chefToDos}
  }
  return state;
}