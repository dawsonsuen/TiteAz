import * as React from 'react'
import { connect, Link } from 'cerebral-view-react'
import { getPath } from 'models/StateModel'
import { Views } from 'models/ViewsStateModel'
import PropSignals from 'models/Signals'
import { ViewPort, View } from './ViewPort';

interface Props extends PropSignals {

}

export default connect<Props>({

}, function Nav(props) {
    return (
        <nav className="navbar navbar-inverse navbar-fixed-top">
            <div className="container-fluid" >
                <div className="navbar-header">
                    <a className="navbar-brand" href="/#/">
                        Brand Here
                    </a>
                </div>
                <div className="collapse navbar-collapse">
                    <ul className="nav navbar-nav">
                        {
                        /*
                        <li className={props.view == Views.Home ? "active" : ""}>
                            <Link signal={props.signals.views.OpenHomeView}>
                                Home
                            </Link>
                        </li>
                       */
                       }
                    </ul>
                </div>
            </div>
        </nav>
    )
})

