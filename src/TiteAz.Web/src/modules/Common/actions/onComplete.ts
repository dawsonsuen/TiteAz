import { IActionContext } from "models/StateModel";

export interface ActionInput {
  onComplete: () => void
}

function onComplete({input, state}: IActionContext<ActionInput, {}>) {
  input.onComplete();
}

export default onComplete