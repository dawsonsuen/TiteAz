import React from "react";
import { connect } from 'cerebral-view-react'
import PropSignals from 'models/Signals'
import { getPath } from 'models/StateModel'

interface Props extends PropSignals {
}

export default connect<Props>({
}, function DashboardView(props) {

    return (<div>
            <div> Hello User </div>
        </div>    
    );
});