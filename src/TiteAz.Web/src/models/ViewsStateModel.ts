export enum Views {
    Index = <any>"index",
    Home = <any>"home"
}

export interface PageInfo {
    isInitialized: boolean,
    isChildInitialized?: boolean
}

interface ViewsStateModel {
    selected: Views,
    viewInfo: { [id: string]: PageInfo }
};

export default ViewsStateModel;