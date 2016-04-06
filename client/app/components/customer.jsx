import React from 'react';

var Table = ({table}) => (
  <p>{table.number}, {table.status}</p>
)

class Customer extends React.Component {
  render () {
    return (
      <div>
        <p> Hello Customer!</p>
        <Table table={{number : 1, status : 'open'}} />
      </div>
    )
  }
}

export default Customer