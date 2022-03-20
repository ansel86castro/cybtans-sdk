import * as React from 'react';
import { RouteComponentProps, withRouter } from 'react-router';

interface LoginProps extends RouteComponentProps<{}> {

}

interface LoginState {
    username?: string;
    password?: string;
    errorMessage?: string;
}

class Login extends React.PureComponent<LoginProps, LoginState> {
    constructor(props: LoginProps) {
        super(props);

        this.state = {};
    }

    onInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ [e.target.name]: e.target.value });
    }

    onSubmit = (e: React.FormEvent<HTMLButtonElement>) => {
        e.preventDefault();


        fetch('api/login', {
            method: 'POST',
            body: JSON.stringify({ Username: this.state.username, Password: this.state.password }),
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        })
        .then(
            response => {
                if (response.status == 200) {
                    this.props.history.push('/');
                } else {
                    this.setState({ errorMessage: 'Authentication fail' });
                }
            },
            error => {
                this.setState({ errorMessage: "logging off fail" });
            }
        );
        return false;
    }

    onSignOff = (e: React.FormEvent<HTMLButtonElement>) => {
        e.preventDefault();

        fetch('api/login/logoff', {
            method: 'GET',          
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        })
        .then(
            response => {
                if (response.status == 200) {
                    this.props.history.push('/');
                } else {
                    this.setState({ errorMessage: 'Authentication fail' });
                }
            },
            error => {
                this.setState({ errorMessage: "logging off fail" });
            }
        );

        return false;
    }

    render() {
        return (
            <form>
                {this.state.errorMessage && <div className="alert alert-danger" role="alert">{this.state.errorMessage}</div>}
                <div className="form-group">
                    <label htmlFor="exampleInputEmail1">Username</label>
                    <input name="username" onChange={this.onInputChange} className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Enter Username" />
                </div>

                <div className="form-group">
                    <label htmlFor="exampleInputPassword1">Password</label>
                    <input name="password" onChange={this.onInputChange} type="password" className="form-control" id="exampleInputPassword1" placeholder="Password" />
                </div>
                <button className="btn btn-primary" onClick={this.onSubmit}>Sign in</button>  
                
                <button className="btn btn-secundary" onClick={this.onSignOff}>Sign Off</button>
            </form>
        );
    }
}

export default Login;