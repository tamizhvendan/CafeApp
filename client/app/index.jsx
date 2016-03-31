import React from 'react';
import {render} from 'react-dom';

class App extends React.Component {
  render () {
    return <p> Hello React!</p>;
  }
}
const root = document.createElement('div')
document.body.appendChild(root)
render(<App/>, root);