import React from 'react';
import {Well,Label, Button} from 'react-bootstrap';

class Item extends React.Component {
  onItemClick() {
      this.props.onItemClick(this.props.item.menuNumber)
  }

  render() {
    let item = this.props.item;
    return (
        <Well>
          <h3 style={{display : 'inline'}}>{item.name}</h3>
          <div style={{display : 'inline', float : 'right'}}>
            <Button bsStyle="success" bsSize="small" onClick={this.onItemClick.bind(this)}>{this.props.buttonLabel}</Button>
          </div>
        </Well>
    );
  }
}

export default Item;
