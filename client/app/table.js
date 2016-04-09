import {TabOpened} from './events.js';
import update from 'react-addons-update';

const initialTablesState = {
  tables : []
}

const initialOpenTablesState = {
  openTables : []
}

const TableListSuccess = "TABLE_LIST_SUCCESS"
const OpenTablesListSuccess = "OPEN_TABLES_LIST_SUCCESS"

export function listTables (tables) {
  return {
    type : TableListSuccess,
    tables : tables
  }
}

export function listOpenTables(openTables) {
  return {
    type : OpenTablesListSuccess,
    openTables : openTables
  }
}

export function tablesReducer (state = initialTablesState, action) {
  if (action.type === TableListSuccess) {
    return action.tables;
  }

  if (action.type === TabOpened) {
    let newStatus = {
        inService: action.data.id
    }

    let tables =
      state.tables.map(table => {
        if (table.number === action.data.tableNumber) {
          return update(table, {status : {$set : newStatus}})
        }
        return table
      })
    return {tables : tables}
  }

  return state;
}

export function openTablesReducer(state = initialOpenTablesState, action) {
  if (action.type === OpenTablesListSuccess) {
    return action.openTables
  }
  if (action.type === TabOpened) {
    return {openTables : state.openTables.concat({tableNumber: action.data.tableNumber, tabId: action.data.id})};
  }
  return state
}