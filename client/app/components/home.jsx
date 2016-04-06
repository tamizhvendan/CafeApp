import React from 'react';
import { connect } from 'react-redux';
import axios from 'axios';
import store from './../store.js'

var Table = ({table}) => {
  let status = table.status.open ? "open" : "closed"
  return <p>{table.number}, {status}</p>
}

class Home extends React.Component {
  render () {
    var tables = this.props.tables.map(table => <Table table={table} key={table.number}/>);

    return (
      <div>
        {tables}
      </div>
    )
  }
}

class HomeContainer extends React.Component {
  componentDidMount(){
    axios.get('/tables').then(response => {
      store.dispatch({
        type : 'TABLE_LIST_SUCCESS',
        tables : response.data
      });
    });
  }

  render () {
    return <Home tables={this.props.tables} />;
  }
}

const mapStateToProps = function(store) {
  return {
    tables: store.tablesState.tables
  };
}

export default connect(mapStateToProps)(HomeContainer)