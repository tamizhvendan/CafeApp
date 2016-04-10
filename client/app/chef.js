import {OrderPlaced} from './events.js'

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
  return state;
}