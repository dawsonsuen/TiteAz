import fieldChanged, { ChainInput as fieldChangedInput } from './chains/fieldChanged'
import LoginSignal, { ChainInput as loginInput } from './chains/login'

export interface CommonSignals {
  fieldChanged: <T>(input: fieldChangedInput<T>) => void
  login: (input: loginInput) => void
}

export default module => {

  module.addSignals({
    fieldChanged: {
        chain: fieldChanged,
        immediate: true
    },
    login: LoginSignal
  })
}