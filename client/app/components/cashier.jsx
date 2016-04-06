import React from 'react';

var Payment = ({payment}) => (
  <p>{payment.tableNumber}, {payment.amount}</p>
)

class Cashier extends React.Component {
  render () {
    return (
      <div>
        <p> Hello Cashier!</p>
        <Payment payment={{tableNumber : 1, amount : 17.5}} />
      </div>
    )
  }
}

export default Cashier