import updateValue from '../actions/updateValue'

export interface ChainInput<T> {
  field: T
  urlField?: string
  value: T
}

export default [
  updateValue
]
