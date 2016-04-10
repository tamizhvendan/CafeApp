import React from 'react';
import Item from './item.jsx';
import { connect } from 'react-redux';
import axios from 'axios';
import store from './../store.js'
import {listWaiterToDos} from './../waiter.js'
import {Grid, Row, Col, Panel} from 'react-bootstrap';


class WaiterToDo extends React.Component {
  onFoodServed(menuNumber) {
    this.props.onFoodServed(this.props.waiterToDo.tabId, menuNumber)
  }

  render() {
    let waiterToDo = this.props.waiterToDo;
    let panelTitle = `Table Number ${waiterToDo.tableNumber}`;
    let foodItems =
      waiterToDo.foodItems.map(foodItem =>
                                (<Item item={foodItem}
                                    buttonLabel="Mark Served"
                                    onItemClick={this.onFoodServed.bind(this)}
                                    key={foodItem.menuNumber}/>))
    return(
      <Col md={4}>
        <Panel header={panelTitle} bsStyle="primary">
          {foodItems}
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

  render () {
    let todos =
      this.props.waiterToDos.map(waiterToDo => <WaiterToDo
                                              key={waiterToDo.tabId}
                                              waiterToDo={waiterToDo}
                                              onFoodServed={this.onFoodServed}/>)
    return (
        <Grid>
          <Row>{todos}</Row>
        </Grid>
    )
  }
}
const mapStateToProps = function(store) {
  return {
    waiterToDos: store.waiterToDosState.waiterToDos
  };
}

export default connect(mapStateToProps)(Waiter)