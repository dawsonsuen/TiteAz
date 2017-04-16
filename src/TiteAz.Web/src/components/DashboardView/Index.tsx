import React from "react";
import { connect } from 'cerebral-view-react'
import PropSignals from 'models/Signals'
import { getPath } from 'models/StateModel'

interface Props extends PropSignals {
    name : string
}

export default connect<Props>({
    name : getPath(x => x.login.loggedInUser)
}, function DashboardView(props) {

    return (<div>
            <div> Hello {props.name} </div>
        </div>    
    );
});