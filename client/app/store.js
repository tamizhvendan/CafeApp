import { createStore, combineReducers } from 'redux';
import {tablesReducer, openTablesReducer} from './table.js';
import {chefToDosReducer} from './chef.js';
import {cashierToDosReducer} from './cashier.js';
import {waiterToDosReducer} from './waiter.js';
import {drinksReducer, foodsReducer} from './items.js';

const reducers = combineReducers({
  tablesState : tablesReducer,
  openTablesState : openTablesReducer,
  chefToDosState : chefToDosReducer,
  waiterToDosState : waiterToDosReducer,
  cashierToDosState : cashierToDosReducer,
  foodsState : foodsReducer,
  drinksState : drinksReducer
})

const store = createStore(reducers);

export default store