import React from "react";
import { connect } from 'cerebral-view-react'
import PropSignals from 'models/Signals'
import { getPath } from 'models/StateModel'

interface Props extends PropSignals {
    username: string
    password: string
}

export default connect<Props>({
    username: getPath(x => x.login.username),
    password: getPath(x => x.login.password)
}, function LoginView(props) {
    const onChange = (field: string, value: string) => {
        props.signals.common.fieldChanged({
            field: field,
            value: value
        })
    }

    let onUsernameChange = (event) => {
        onChange(getPath(x => x.login.username), event.target.value);
    }

    let onPasswordChange = (event) => {
        onChange(getPath(x => x.login.password), event.target.value);
    }

    let login = () => {
        props.signals.common.login({});
    }

    return (
        <div>
        <div>Login </div>

        <div>
            <div className="input-group">
                <span className="input-group-addon">Username</span>                
            <input className="form-control" name="username" type="text" value={props.username} onChange={onUsernameChange} />
            </div>
                <div className="input-group">
            <span className="input-group-addon">Password</span>                
            <input className="form-control" name="password" type="password" value={props.password} onChange={onPasswordChange} />
            </div>   
              <button className="btn btn-default" onClick={login}> Login </button>
          </div>     
          </div>     
    );
});