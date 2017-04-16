import { Views } from "models/ViewsStateModel";
import { getPath } from "models/StateModel";
import { set, when } from 'cerebral/operators';

export default [
    set(getPath(x=>x.views.selected), Views.Login)
]