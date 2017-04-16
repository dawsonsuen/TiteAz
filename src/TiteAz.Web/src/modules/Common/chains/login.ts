import login from '../actions/login'
import { Views } from "models/ViewsStateModel";
import { getPath } from "models/StateModel";
import { set, when } from 'cerebral/operators';

export interface ChainInput {
}

export default [
  login, {
    success: [
      set(getPath(x=>x.views.selected), Views.Dashboard)
    ],
    error: [

    ]
  }
]
