import React from 'react';
import Item from './item.jsx';
import { connect } from 'react-redux';
import axios from 'axios';
import store from './../store.js'
import {listWaiterToDos} from './../waiter.js'
import {Grid, Row, Col, Panel, Alert} from 'react-bootstrap';


class WaiterToDo extends React.Component {
  onFoodServed(menuNumber) {
    this.props.onFoodServed(this.props.waiterToDo.tabId, menuNumber)
  }

  onDrinkServed(menuNumber) {
    this.props.onDrinkServed(this.props.waiterToDo.tabId, menuNumber)
  }

  toItemView(item, handler) {
    return (<Item item={item}
              buttonLabel="Mark Served"
              onItemClick={handler}
              key={item.menuNumber} />);
  }

  render() {
    let waiterToDo = this.props.waiterToDo;
    let panelTitle = `Table Number ${waiterToDo.tableNumber}`;
    let foods =
      waiterToDo.foods.map(item => this.toItemView(item, this.onFoodServed.bind(this)));
    let drinks =
      waiterToDo.drinks.map(item => this.toItemView(item, this.onDrinkServed.bind(this)));


    return(
      <Col md={4}>
        <Panel header={panelTitle} bsStyle="primary">
          {foods}
          {drinks}
        </Panel>
      </Col>
    );
  }
}

class Waiter extends React.Component {
  componentDidMount(){
    axios.get('/todos/waiter').then(response => {
      store.dispatch(listWaiterToDos(response.data));
    });
  }

  onFoodServed(tabId, menuNumber){
    axios.post('/command', {
      serveFood : {
        tabId,
        menuNumber
      }
    })
  }

  onDrinkServed(tabId, menuNumber){
    axios.post('/command', {
      serveDrink : {
        tabId,
        menuNumber
      }
    })
  }

  render () {
    let todos =
      this.props.waiterToDos.map(waiterToDo => <WaiterToDo
                                              key={waiterToDo.tabId}
                                              waiterToDo={waiterToDo}
                                              onFoodServed={this.onFoodServed}
                                              onDrinkServed={this.onDrinkServed}/>)
    let view =
      todos.length
      ? <Grid><Row>{todos}</Row></Grid>
      : <Alert bsStyle="warning">No Items Available For Serving</Alert>

    return view;
  }
}
const mapStateToProps = function(store) {
  return {
    waiterToDos: store.waiterToDosState.waiterToDos
  };
}

export default connect(mapStateToProps)(Waiter)