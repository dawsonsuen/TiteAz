interface LoginStateModel {
    username: string
    password: string
    loggedInUser: User
};

export interface User {
    id: string
    email: string
    firstName: string
    lastName: string
};

export default LoginStateModel;