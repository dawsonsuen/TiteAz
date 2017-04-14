import { pathFromModel } from "../helpers/common"
import { CommonSignals } from 'modules/Common';
import { ViewsSignals } from "modules/Views";


export interface AppSignals {
    common: CommonSignals,
    views: ViewsSignals
}

export interface ISignals {
    signals?: AppSignals
}

export interface PropsSignals extends ISignals {}

export const getSignal = pathFromModel<AppSignals>();

export default PropsSignals;