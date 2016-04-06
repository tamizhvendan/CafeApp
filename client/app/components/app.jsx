import React from 'react';
import {Link} from 'react-router';
import AppNavBar from './appNavBar.jsx'

class App extends React.Component {

  render () {
    return (
      <div>
        <AppNavBar />
        <div className="well">
          {this.props.children}
        </div>
      </div>
    );
  }
}

export default App