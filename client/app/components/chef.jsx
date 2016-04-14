import React from 'react';
import Item from './item.jsx';
import { connect } from 'react-redux';
import axios from 'axios';
import store from './../store.js'
import {listChefToDos} from './../chef.js'
import {Grid, Row, Col, Panel, Alert} from 'react-bootstrap';


class ChefToDo extends React.Component {
  onFoodPrepared(menuNumber) {
    this.props.onFoodPrepared(this.props.chefToDo.tabId, menuNumber)
  }

  render() {
    let chefToDo = this.props.chefToDo;
    let panelTitle = `Table Number ${chefToDo.tableNumber}`;
    let foodItems =
      chefToDo.foodItems.map(foodItem =>
                                (<Item item={foodItem}
                                    buttonLabel="Mark Prepared"
                                    onItemClick={this.onFoodPrepared.bind(this)}
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

class Chef extends React.Component {
  componentDidMount(){
    axios.get('/todos/chef').then(response => {
      store.dispatch(listChefToDos(response.data));
    });
  }

  onFoodPrepared(tabId, menuNumber){
    axios.post('/command', {
      prepareFood : {
        tabId,
        menuNumber
      }
    })
  }

  render () {
    let todos =
      this.props.chefToDos.map(chefToDo => <ChefToDo
                                              key={chefToDo.tabId}
                                              chefToDo={chefToDo}
                                              onFoodPrepared={this.onFoodPrepared}/>)

    let view =
      todos.length
      ? <Grid><Row>{todos}</Row></Grid>
      : <Alert bsStyle="warning">No Foods Available For Preparation</Alert>

    return view;
  }
}
const mapStateToProps = function(store) {
  return {
    chefToDos: store.chefToDosState.chefToDos
  };
}

export default connect(mapStateToProps)(Chef)