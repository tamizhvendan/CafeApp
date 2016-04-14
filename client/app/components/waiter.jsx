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

  onDrinksServed(menuNumber) {
    this.props.onDrinksServed(this.props.waiterToDo.tabId, menuNumber)
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
    let foodItems =
      waiterToDo.foodItems.map(item => this.toItemView(item, this.onFoodServed.bind(this)));
    let drinksItems =
      waiterToDo.drinksItems.map(item => this.toItemView(item, this.onDrinksServed.bind(this)));


    return(
      <Col md={4}>
        <Panel header={panelTitle} bsStyle="primary">
          {foodItems}
          {drinksItems}
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

  onDrinksServed(tabId, menuNumber){
    axios.post('/command', {
      serveDrinks : {
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
                                              onDrinksServed={this.onDrinksServed}/>)
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