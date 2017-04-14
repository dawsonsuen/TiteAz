import { HttpModule } from "cerebral-module-http";
import { RouterModule } from "cerebral-module-router";
import { pathFromModel } from "../helpers/common";
import ViewsStateModel from "./ViewsStateModel";
import { IStateModel } from "cerebral";

interface StateModel {
    views :ViewsStateModel
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