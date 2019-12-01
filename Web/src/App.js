import React, { Component } from 'react';
import Login from './Login';
import NavBar from './NavBar';
import NewUserForm from './NewUserForm';
import {
  BrowserRouter as Router,
  Switch,
  Route,
} from 'react-router-dom';


class App extends Component {
  render () {
    return(
      <Router>
        <div>
          <NavBar />
        </div>
        <div>
          <Switch>
            <Route exact path="/">
              <Login />
            </Route>
            <Route exact path="/NewUserForm">
              <NewUserForm />
            </Route>
          </Switch>
        </div>
      </Router>
    );
  }
}

export default App;
