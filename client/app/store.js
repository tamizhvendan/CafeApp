import { createStore, combineReducers } from 'redux';

const initialTablesState = {
  tables : []
}

const tablesReducer = function (state = initialTablesState, action) {
  if (action.type === "TABLE_LIST_SUCCESS") {
    return action.tables;
  }
  return state;
}

const intialChefTodosState = {
  chefToDos : []
}

const chefToDosReducer = function(state = intialChefTodosState, action) {
  if (action.type === "CHEF_TODO_LIST_SUCCESS") {
    return action.chefToDos;
  }
  return state;
}

const reducers = combineReducers({
  tablesState : tablesReducer,
  chefToDosState : chefToDosReducer
})

const store = createStore(reducers);

export default store