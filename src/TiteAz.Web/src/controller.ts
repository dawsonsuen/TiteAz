import Model from 'cerebral-model-immutable'
import { Controller } from 'cerebral'
import { getSignal, ISignals } from './models/Signals'
import StateModel from './models/StateModel'
import Devtools from 'cerebral-module-devtools'
import Router from 'cerebral-module-router'
import Http from 'cerebral-module-http'

import Common from 'modules/Common'
import { Views } from "models/ViewsStateModel";
import ViewsSignals from './modules/Views';

declare var process: any;

const appState: StateModel = {
    views:{
        selected: Views.Index,
        viewInfo:{}
    }
};

const controller = Controller(Model(appState, { immutable: true }))

var modules = {

    common: Common,
    views: ViewsSignals,
    http: Http({
        baseUrl: '/api',
    }),
    router: Router({
        '/': getSignal(x => x.views.OpenIndex) as any,

    }, {
        onlyHash: true, // use only hash part of url for matching
        query: true     // option to enable query support in url-mapper
    })
};

if (process.env.NODE_ENV !== 'production') modules["devtools"] = Devtools();

controller.addModules(modules);

export default controller