import React from 'react';
import { connect } from 'react-redux';
import axios from 'axios';
import store from './../store.js'
import Table from './table.jsx'
import {listTables} from './../table.js'
import {Grid, Row, Col} from 'react-bootstrap';

class Home extends React.Component {

  toTableView(table, handler) {
    return (
      <Col md={4} key={table.number}>
        <Table table={table} onTableClick={handler}/>
      </Col>
    )
  }

  render () {
    var tables = this.props.tables.map(table => this.toTableView(table, this.props.onTableClick));
    return (
      <Grid>
        <Row>
          {tables}
        </Row>
      </Grid>
    )
  }
}

class HomeContainer extends React.Component {
  componentDidMount(){
    axios.get('/tables').then(response => {
      store.dispatch(listTables(response.data));
    });
  }

  openTab(tableNumber) {
    axios.post('/command', {
      openTab : {
        tableNumber
      }
    });
  }

  render () {
    return <Home tables={this.props.tables} onTableClick={this.openTab} />;
  }
}

const mapStateToProps = function(store) {
  return {
    tables: store.tablesState.tables
  };
}

export default connect(mapStateToProps)(HomeContainer)