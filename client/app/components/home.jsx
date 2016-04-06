import React from 'react';
import { connect } from 'react-redux';

var Table = ({table}) => (
  <p>{table.number}, {table.status}</p>
)

class Home extends React.Component {
  render () {
    var tables = this.props.tables;
    return (
      <div>
        <Table table={tables[0]} />
        <Table table={tables[1]} />
      </div>
    )
  }
}

// class HomeContainer extends React.Component {
//
// }

const mapStateToProps = function(store) {
  return {
    tables: store.tablesState.tables
  };
}

export default connect(mapStateToProps)(Home)