import * as React from 'react'
import { connect } from 'cerebral-view-react'
import { getPath } from 'models/StateModel'
import { Views } from 'models/ViewsStateModel'
import PropSignals from 'models/Signals'
import { ViewPort, View } from './ViewPort';
import Nav from "./nav";
import DashboardView from "components/DashboardView";
import LoginView from "components/LoginView";


interface Props extends PropSignals {
    currentView :Views
}

export default connect<Props>({
    currentView: getPath(x=>x.views.selected)
}, function App(props) {

    return (
        <div className="wrapper">
            <Nav />
            <section id="main">
                <div className="inner-wrapper">
                    <ViewPort view={props.currentView}>
                        <View match={Views.Dashboard}>
                            <DashboardView />
                        </View>
                        <View match={Views.Login}>
                            <LoginView />
                        </View>
                    </ViewPort>
                </div>
            </section>
        </div>
    )
})

