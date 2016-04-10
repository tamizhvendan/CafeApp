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
  return state;
}