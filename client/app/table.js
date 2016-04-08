import {TabOpened} from './events.js';
import update from 'react-addons-update';

const initialTablesState = {
  tables : []
}

const TableListSuccess = "TABLE_LIST_SUCCESS"

export function listTables (tables) {
  return {
    type : TableListSuccess,
    tables : tables
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