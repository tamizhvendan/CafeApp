import React from 'react';
import {connect} from 'react-redux';
import {Grid, Row, Col, ButtonInput, Input, PageHeader} from 'react-bootstrap';
import axios from 'axios';
import store from './../store.js';
import {listOpenTables} from './../table.js';

class Order extends React.Component {
  onSubmit(event) {
    let order = {
      tabId : this.props.table.tabId,
      foodMenuNumbers : this.refs.foods.getValue(),
      drinksMenuNumbers : this.refs.drinks.getValue()
    }
    event.preventDefault();
    console.log(order)
  }
  render() {
    let toOption = (item => <option key={item.menuNumber} value={item.menuNumber}>{item.name}</option>);
    var foods = this.props.foods.map(toOption);
    var drinks = this.props.drinks.map(toOption);
    return (
      <Col md={5} className="well" style={{margin: '15px'}}>
        <PageHeader>Table {this.props.table.tableNumber}</PageHeader>
        <form className="form-horizontal">
          <Input type="select" label="Food Items" multiple ref="foods" labelClassName="col-xs-4" wrapperClassName="col-xs-7">
            {foods}
          </Input>
          <Input type="select" label="Drinks" multiple ref="drinks" labelClassName="col-xs-4" wrapperClassName="col-xs-7">
            {drinks}
          </Input>
          <ButtonInput bsStyle="primary" type="submit" value="Place Order"  bsSize="large" wrapperClassName="col-xs-offset-7 col-xs-7" />
        </form>
      </Col>
    );
  }
}

class OrdersPage extends React.Component {
  componentDidMount () {
    axios.get("/tables").then(response => {
      let tables = response.data.tables
      let openTables =
        tables.filter(table => table.status.open)
        .map(table => ({tableNumber : table.number, tabId : table.status.open}))
      store.dispatch(listOpenTables({openTables : openTables}))
    });
  }

  render () {
    let orders =
      this.props.openTables
      .map(table => <Order key={table.tabId} table={table} foods={this.props.foods} drinks={this.props.drinks} />)
      return (
        <Grid>
          <Row>
            {orders}
          </Row>
        </Grid>);
  }
}

const mapStateToProps = function(store) {
  return {
    foods: store.foodsState.foods,
    drinks: store.drinksState.drinks,
    openTables: store.openTablesState.openTables
  };
}

export default connect(mapStateToProps)(OrdersPage);