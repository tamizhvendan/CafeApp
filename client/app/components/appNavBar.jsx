import React from 'react';
import {Link} from 'react-router'
import {Navbar, Nav, NavItem} from 'react-bootstrap';
import {LinkContainer, IndexLinkContainer} from 'react-router-bootstrap';

var AppNavBar = (props) => {
  return(
    <Navbar static>
     <Navbar.Header>
       <Navbar.Brand>
         <Link to="/">F# Cafe</Link>
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
         <LinkContainer to={{pathname : "orders"}}>
           <NavItem>Orders</NavItem>
         </LinkContainer>
       </Nav>
     </Navbar.Collapse>
   </Navbar>);
}

export default AppNavBar