import fieldChanged, { ChainInput as fieldChangedInput } from './chains/fieldChanged'

export interface CommonSignals {
  fieldChanged: <T>(input: fieldChangedInput<T>) => void
}

export default module => {

  module.addSignals({
    fieldChanged: {
        chain: fieldChanged,
        immediate: true
    }
  })
}