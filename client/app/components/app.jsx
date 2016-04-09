import React from 'react';
import {Link} from 'react-router';
import AppNavBar from './appNavBar.jsx'
import store from './../store.js';
import {listFoods, listDrinks} from './../items.js';
import axios from 'axios';

class App extends React.Component {

  componentDidMount() {
    axios.get('/foods').then(response => {
      store.dispatch(listFoods(response.data))
    });
    axios.get('/drinks').then(response => {
      store.dispatch(listDrinks(response.data))
    });
  }

  render () {
    return (
      <div>
        <AppNavBar />
        <div>
          {this.props.children}
        </div>
      </div>
    );
  }
}

export default App