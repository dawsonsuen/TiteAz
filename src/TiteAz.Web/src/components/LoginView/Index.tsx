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

    return (<div>
            <div>Login </div>

            <div>
                <label htmlFor="username"> Username  </label> &nbsp;                
                <input id="username" name="username" type="text" value={props.username} onChange={onUsernameChange} />
                <br />
                <label htmlFor="password"> Password </label> &nbsp;
                <input id="password" name="password" type="password" value={props.password} onChange={onPasswordChange} />
                <br />
                <button onClick={login}> Login </button>
            </div>   
        </div>    
    );
});