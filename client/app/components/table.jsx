import React from 'react';
import {PageHeader, Button} from 'react-bootstrap';

class Table extends React.Component {

  onTableClick () {
    this.props.onTableClick(this.props.table.number);
  }

  getTableStatus(table) {
    if (table.status.open) {
      return <Button bsSize="large" block active disabled>Waiting for Order</Button>;
    } else if (table.status.inService) {
      return <Button bsSize="large" block active disabled>In Service</Button>;
    }
    return ( <Button bsStyle="primary" bsSize="large" block onClick={this.onTableClick.bind(this)}>
          Take
        </Button> )
  }

  render () {
    let table = this.props.table
    return (
      <div className="well" style={{margin: '0 auto 10px'}}>
        <PageHeader>Table {this.props.table.number}</PageHeader>
        {this.getTableStatus(this.props.table)}
      </div>
    )
  }
}

export default Table;