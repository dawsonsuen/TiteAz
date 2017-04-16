import { IActionContext, getPath } from "models/StateModel";

function setUser({input, state}: IActionContext<any, {}>) {
    state.set(getPath(x=>x.login.loggedInUser), input.userDetails.firstName)
}

export default setUser