import ViewsStateModel, { Views } from "models/ViewsStateModel";
import OpenIndex from "./chains/openIndex";

export interface ViewsSignals {
    OpenIndex(): () => void;
}

export default (module, controller) => {
    const initialState: ViewsStateModel = {
        selected: Views.Index,
        viewInfo: {}
    };

    module.addState(initialState);

    module.addSignals({
        OpenIndex : OpenIndex
    });
}
