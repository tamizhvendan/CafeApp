import {TabOpened, OrderPlaced} from './events.js';
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

function changeTableStatusByNumber(tables, tableNumber, newStatus){
  return tables.map(table => {
              if (table.number === tableNumber) {
                return update(table, {status : {$set : newStatus}})
              }
              return table;});
}

function changeTableStatusByTabId(tables, tabId, newStatus){
  return tables.map(table => {
              if (table.status.open === tabId) {
                return update(table, {status : {$set : newStatus}})
              }
              return table;});
}

export function tablesReducer (state = initialTablesState, action) {
  if (action.type === TableListSuccess) {
    return action.tables;
  }
  if (action.type === TabOpened) {
    let newStatus = {open: action.data.id};
    let tables = changeTableStatusByNumber(state.tables, action.data.tableNumber, newStatus)
    return {tables}
  }
  if (action.type === OrderPlaced) {
    let newStatus = {inService: action.data.tabId};
    let tables = changeTableStatusByTabId(state.tables, action.data.tabId, newStatus)
    return {tables}
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