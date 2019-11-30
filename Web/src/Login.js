import React, { Component } from 'react';
import './Login.css';
import {Link} from 'react-router-dom';

class Login extends Component {
    constructor() {
        super();

        this.state = {
            username: '',
            password: '',
            error: '',
        };

        this.handlePassChange = this.handlePassChange.bind(this);
        this.handleUserChange = this.handleUserChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.dismissError = this.dismissError.bind(this);
    }

    dismissError() {
        this.setState({ error: '' });
    }

    handleSubmit(event) {
        event.preventDefault();

        if (!this.state.username) {
            return this.setState({ error: 'Username is required' });
        }

        if (!this.state.password) {
            return this.setState({ error: 'Password is required' });
        }

        return this.setState({ error: '' });
    }

    handleUserChange(event) {
        this.setState({
            username: event.target.value,
        });
    };

    handlePassChange(event) {
        this.setState({
            password: event.target.value,
        });
    }

    render() {
        return(
            <div className="login-home">
                <div className="login-header">
                    <h1>Login</h1>
                </div>
                <div className="form">
                    <form onSubmit={this.handleSubmit}>
                        {
                            this.state.error &&
                            <h3 data-test="error" onClick={this.dismissError}>
                                <button onClick={this.dismissError}>* {this.state.error}</button>
                            </h3>
                        }

                        <input id="username" data-test="username" type="text" placeholder="Username" value={this.state.username} onChange={this.handleUserChange} />

                        <input id="password" data-test="password" type="password" placeholder="Password" value={this.state.password} onChange={this.handlePassChange} />

                        <input id="submit" type="submit" value="Login" data-test="submit" />
                    </form>
                    <br />
                    <hr />
                </div>
                <div className="newuserlink">
                    <Link to="/NewUserForm">Create New Account</Link>
                </div>
            </div>
        );
    }
}


export default Login;