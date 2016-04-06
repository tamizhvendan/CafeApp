import { createStore, combineReducers } from 'redux';

const initialTablesState = {
  tables : [{number : 1, status : 'open'}, {number : 2, status : 'open'}]
}

const tablesReducer = function (state = initialTablesState, action) {
  return state;
}

const reducers = combineReducers({
  tablesState : tablesReducer
})

const store = createStore(reducers);

export default store