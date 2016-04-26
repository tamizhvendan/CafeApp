import {TabClosed, OrderServed} from './events.js';

const initialCashierToDosSate = {
  cashierToDos : []
}

const CashierToDoListSuccess = "CASHIER_TODO_LIST_SUCCESS"

export function listCashierToDos(cashierToDos){
  return {
    type : CashierToDoListSuccess,
    cashierToDos : cashierToDos
  }
}

export function cashierToDosReducer(state = initialCashierToDosSate, action) {
  if (action.type === CashierToDoListSuccess) {
    return action.cashierToDos;
  }

  if (action.type === OrderServed) {
    let todo = { tabId : action.data.tabId,
                  paymentAmount: action.data.amount,
                  tableNumber: action.data.tableNumber};
    let cashierToDos = state.cashierToDos.concat(todo);
    return {cashierToDos}
  }
  if (action.type === TabClosed) {
    let cashierToDos = state.cashierToDos.filter(todo => todo.tabId !== action.data.tabId)
    return {cashierToDos};
  }
  return state;
}