import React from 'react';
import {Link} from 'react-router'
import {Navbar, Nav, NavItem} from 'react-bootstrap';
import {LinkContainer, IndexLinkContainer} from 'react-router-bootstrap';

var AppNavBar = (props) => {
  return(
    <Navbar>
     <Navbar.Header>
       <Navbar.Brand>
         <Link to="/">CafeApp</Link>
       </Navbar.Brand>
       <Navbar.Toggle />
     </Navbar.Header>
     <Navbar.Collapse className="bs-navbar-collapse">
       <Nav role="navigation" id="top">
         <LinkContainer to={{pathname : "chef"}}>
           <NavItem>Chef</NavItem>
         </LinkContainer>
         <LinkContainer to={{pathname : "waiter"}}>
           <NavItem>Waiter</NavItem>
         </LinkContainer>
         <LinkContainer to={{pathname : "cashier"}}>
           <NavItem>Cashier</NavItem>
         </LinkContainer>
         <IndexLinkContainer to="/">
           <NavItem>Customer</NavItem>
         </IndexLinkContainer>
       </Nav>
     </Navbar.Collapse>
   </Navbar>);
}

export default AppNavBar