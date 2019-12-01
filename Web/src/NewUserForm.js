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

    dissmissError() {
        this.setState({ error: '' });
    }

    handleSubmit(event) {
        event.preventDefault();
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

                    <input id="username" name="username" type="text" placeholder="Username" htmlFor="password" />

                    <input id="email" name="email" type="email" placeholder="Email" htmlFor="email" />

                    <input id="password" name="password" type="password" placeholder="Password" htmlFor="password" />

                    <button>Submit</button>
                </form>
                </div>
            </div>
        );
    }
}

export default NewUserForm;