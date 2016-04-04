import {render} from 'react-dom';
import { Router, Route, IndexRoute , Link, browserHistory } from 'react-router'
import React from 'react';
import App from './components/app.jsx';
import Customer from './components/customer.jsx'
import Cashier from './components/cashier.jsx'
import Waiter from './components/waiter.jsx'
import Chef from './components/chef.jsx'


render((
  <Router history={browserHistory}>
    <Route path="/" component={App}>
      <IndexRoute component={Customer} />
      <Route path="cashier" component={Cashier}/>
      <Route path="chef" component={Chef}/>
      <Route path="waiter" component={Waiter}/>
      <Route path="customer" component={Customer}/>
    </Route>
  </Router>
), document.getElementById("app"))