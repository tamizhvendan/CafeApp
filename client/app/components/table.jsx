import React from 'react';
import {PageHeader, Button} from 'react-bootstrap';

class Table extends React.Component {

  onTableClick () {
    this.props.onTableClick(this.props.table.number);
  }

  statusView() {
    return (
      this.props.table.status.open
      ? <Button bsSize="large" block active>In Use</Button>
      : ( <Button bsStyle="primary" bsSize="large" block onClick={this.onTableClick.bind(this)}>
            Take
          </Button> )
    )
  }

  render () {
    let table = this.props.table
    return (
      <div className="well" style={{margin: '0 auto 10px'}}>
        <PageHeader>Table {this.props.table.number}</PageHeader>
        {this.statusView()}
      </div>
    )
  }
}

export default Table;