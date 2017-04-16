import { ChainInput } from '../chains/fieldChanged'
import { IActionContext, getPath } from "models/StateModel";  

function login({input, state, services, output}: IActionContext<ChainInput<any>, any>) {
  let loginDetails = state.get(getPath(x => x.login))  

  services.http.post('/login', loginDetails)
  .then((response) => {
    output.success({ userDetails: response.result });
  }).catch(output.error);
}

(login as any).outputs = ["error", "success"];
(login as any).async = true;

export default login