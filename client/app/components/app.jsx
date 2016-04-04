import React from 'react';
import {Link} from 'react-router';
import {Navbar, Nav} from 'react-bootstrap';

class App extends React.Component {

  render () {
    return (
      <div>
        <p> Hello Fsharp + React!</p>
        <ul>
          <li><Link to="chef">Chef</Link></li>
          <li><Link to="customer">Customer</Link></li>
          <li><Link to="waiter">Waiter</Link> </li>
          <li><Link to="cashier">Cashier</Link></li>
        </ul>
        <div>
          {this.props.children}
        </div>
      </div>
    );
  }
}

export default App