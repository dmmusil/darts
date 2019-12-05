import React, { Component } from 'react';
import './NewUserForm.css';

class NewUserForm extends Component {
    constructor(props) {
        super(props);

        this.state = {
            username: '',
            email: '',
            password: '',
            error: '',
        };

        this.handlePassChange = this.handlePassChange.bind(this);
        this.handleUserChange = this.handleUserChange.bind(this);
        this.handleEmailChange = this.handleEmailChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.dismissError = this.dismissError.bind(this);
    }

    dismissError() {
        this.setState({ error: '' });
    }

    handleSubmit(event) {
        event.preventDefault();

        if (!this.state.username) {
            return this.setState({ error: 'Username is required'});
        }

        if (!this.state.email) {
            return this.setState({ error: 'Email is required' });
        }

        if (!this.state.password) {
            return this.setState({ error: 'Password is required' });
        }

        const data = new FormData(event.target);
        let jsonObject = {};

        for (const [key, value] of data.entries()){
            jsonObject[key] = value;
        }

        fetch('https://darticorn.azurewebsites.net/api/RegisterUser', {
            method: 'POST',
            body: JSON.stringify(jsonObject),
        }) 
        .then(json => {
            console.log(json)
            window.location.href = "/";
        })
        
        return this.setState({ error: '' });
    }

    handleUserChange(event) {
        this.setState({
            username: event.target.value,
        });
    };

    handleEmailChange(event) {
        this.setState({
            email: event.target.value,
        });
    };

    handlePassChange(event) {
        this.setState({
            password: event.target.value,
        });
    }

    render() {
        return(
            <div className="cya-table">
                <div className="signup-header">
                    <h1>Create your account</h1>
                </div>
                <div className="form">
                <form onSubmit={this.handleSubmit}>
                    {
                        this.state.error &&
                        <h3 data-test="error" onClick={this.dismisError}>
                            <button onClick={this.dismissError}>* {this.state.error}</button>
                        </h3>
                    }

                    <input id="username" data-test="username" type="text" placeholder="Username" value={this.state.username} onChange={this.handleUserChange} />

                    <input id="email" data-test="email" type="email" placeholder="Email" value={this.state.email} onChange={this.handleEmailChange} />

                    <input id="password" data-test="password" type="password" placeholder="Password" value={this.state.password} onChange={this.handlePassChange} />

                    <input id="submit" type="submit" value="Register" data-test="submit" />
                </form>
                </div>
            </div>
        );
    }
}

export default NewUserForm;