import React from 'react';
import axios from 'axios';
import {Grid, Row, Col, Panel, Well,Label, Button} from 'react-bootstrap';
import {connect} from 'react-redux';
import {listCashierToDos} from './../cashier.js';
import store from './../store.js'

class Payment extends React.Component {
  onPaymentClick() {
    let payment = this.props.payment
    this.props.onPaymentClick(payment.tabId, payment.amount);
  }

  render() {
    let payment = this.props.payment;
    return (
        <Well>
          <h3 style={{display : 'inline'}}>Amount : {payment.amount}</h3>
          <div style={{display : 'inline', float : 'right'}}>
            <Button bsStyle="success" bsSize="small" onClick={this.onPaymentClick.bind(this)}>Mark Paid</Button>
          </div>
        </Well>
    );
  }
}

class CashierToDo extends React.Component {
  render () {
    let cashierToDo = this.props.cashierToDo;
    let panelTitle = `Table Number ${cashierToDo.tableNumber}`;
    let payment = {tabId : cashierToDo.tabId, amount : cashierToDo.paymentAmount};

    return (
      <Col md={4}>
        <Panel header={panelTitle} bsStyle="primary">
          <Payment payment={payment} onPaymentClick={this.props.onPaymentClick} />
        </Panel>
      </Col>
    );
  }
}

class Cashier extends React.Component {
  onPaymentClick(tabId, amount) {
    axios.post('/command', {
      closeTab: {
        tabId,
        amount
      }
    })
  }

  componentDidMount() {
    axios.get('/todos/cashier').then(response => {
      store.dispatch(listCashierToDos(response.data))
    });
  }

  render () {
    let todos = this.props.cashierToDos.map(todo => (<CashierToDo
                                                      key={todo.tabId}
                                                      cashierToDo={todo}
                                                      onPaymentClick={this.onPaymentClick}/>));
    return (
      <Grid>
        <Row>{todos}</Row>
      </Grid>
    )
  }
}

const mapStateToProps = function(store) {
  return {
    cashierToDos: store.cashierToDosState.cashierToDos
  };
}

export default connect(mapStateToProps)(Cashier)