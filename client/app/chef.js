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
  return state;
}