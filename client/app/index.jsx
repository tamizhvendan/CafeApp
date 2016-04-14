import {render} from 'react-dom';
import { Router, Route, IndexRoute , Link, browserHistory } from 'react-router'
import React from 'react';
import App from './components/app.jsx';
import Home from './components/home.jsx'
import Cashier from './components/cashier.jsx'
import Waiter from './components/waiter.jsx'
import Chef from './components/chef.jsx'
import Orders from './components/orders.jsx'
import { Provider } from 'react-redux';
import CafeAppWS from './websocket.js';
import store from './store';

function dispatcher (event) {
  store.dispatch({
    type : event.event,
    data : event.data
  })
}

let ws = new CafeAppWS("ws://0.0.0.0:8083/websocket", dispatcher)

const router = (
  <Router history={browserHistory}>
    <Route path="/" component={App}>
      <IndexRoute component={Home} />
      <Route path="cashier" component={Cashier}/>
      <Route path="chef" component={Chef}/>
      <Route path="waiter" component={Waiter}/>
      <Route path="orders" component={Orders}/>
    </Route>
  </Router>
);

render(<Provider store={store}>{router}</Provider>, document.getElementById("app"))