import React from 'react';
import {connect} from 'react-redux';
import {Input} from 'react-bootstrap';

class Orders extends React.Component {
  render() {
    let toOption = (item => <option key={item.menuNumber} value={item.menuNumber}>{item.name}</option>);
    var foods = this.props.foods.map(toOption);
    var drinks = this.props.drinks.map(toOption);
    return (
      <form>
        <Input type="select" label="Food Items" multiple ref="foods">
          {foods}
        </Input>
        <Input type="select" label="Drinks" multiple ref="drinks">
          {drinks}
        </Input>
      </form>
    );
  }
}

const mapStateToProps = function(store) {
  return {
    foods: store.foodsState.foods,
    drinks: store.drinksState.drinks
  };
}

export default connect(mapStateToProps)(Orders);