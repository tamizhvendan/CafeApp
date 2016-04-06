import {render} from 'react-dom';
import { Router, Route, IndexRoute , Link, browserHistory } from 'react-router'
import React from 'react';
import App from './components/app.jsx';
import Home from './components/home.jsx'
import Cashier from './components/cashier.jsx'
import Waiter from './components/waiter.jsx'
import Chef from './components/chef.jsx'
import { Provider } from 'react-redux';
import store from './store';

const router = (
  <Router history={browserHistory}>
    <Route path="/" component={App}>
      <IndexRoute component={Home} />
      <Route path="cashier" component={Cashier}/>
      <Route path="chef" component={Chef}/>
      <Route path="waiter" component={Waiter}/>
    </Route>
  </Router>
);

render(<Provider store={store}>{router}</Provider>, document.getElementById("app"))