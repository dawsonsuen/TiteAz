import { HttpModule } from "cerebral-module-http";
import { RouterModule } from "cerebral-module-router";
import { pathFromModel } from "../helpers/common";
import { IStateModel } from "cerebral";
import ViewsStateModel from "./ViewsStateModel";
import LoginStateModel from "./LoginStateModel";

interface StateModel {
    views: ViewsStateModel,
    login: LoginStateModel
};

export const getPath = pathFromModel<StateModel>();
export const getPath_ = pathFromModel<StateModel>(".*");
export const getPath__ = pathFromModel<StateModel>(".**");

export default StateModel;

 export interface IServices {
    http: HttpModule
    router: RouterModule
}

export interface IActionContext<TInput, TOutput> {
    input: TInput,
    state: IStateModel,
    output: TOutput,
    services: IServices
}