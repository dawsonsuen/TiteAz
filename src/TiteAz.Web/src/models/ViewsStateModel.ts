export enum Views {
    Dashboard = <any>"dashboard",
    Login = <any>"login"
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