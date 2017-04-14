import * as React from 'react'
import { connect } from 'cerebral-view-react'
import { getPath } from 'models/StateModel'
import { Views } from 'models/ViewsStateModel'
import PropSignals from 'models/Signals'
import { ViewPort, View } from './ViewPort';
import Nav from "./nav";
import IndexPage from "components/Index/IndexPage";


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
                        <View match={Views.Index}>
                            <IndexPage />
                        </View>
                    </ViewPort>
                </div>
            </section>
        </div>
    )
})

