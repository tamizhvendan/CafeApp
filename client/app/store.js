import { createStore, combineReducers } from 'redux';
import {tablesReducer} from './table.js';
import {chefToDosReducer} from './chef.js';

const reducers = combineReducers({
  tablesState : tablesReducer,
  chefToDosState : chefToDosReducer
})

const store = createStore(reducers);

export default store