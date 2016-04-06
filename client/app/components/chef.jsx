import React from 'react';
import Item from './item.jsx';

class Chef extends React.Component {
  render () {
    return (
        <div>
          <p> Hello Chef!</p>
          <Item item={{menuNumber: 8, name : "Salad"}}/>
        </div>
    )
  }
}

export default Chef