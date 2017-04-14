declare module "cerebral-view-react" {
    export var Container: any;

    import React, { SFC, ComponentClass, ClassicComponentClass } from 'react';
    type Components<P> = ComponentClass<P> | ClassicComponentClass<P> | SFC<P>;

    export function connect<ExtProps>(stateMap: ExtProps, component: Components<ExtProps>): ClassicComponentClass<any>;
    export function connect<TPropsIn, ExtProps>(stateMap: (props: TPropsIn) => ExtProps, component: Components<ExtProps & TPropsIn>): ClassicComponentClass<TPropsIn>;

    export class Link extends React.Component<LinkProps, any> { }

    interface LinkProps extends React.HTMLProps<HTMLAnchorElement> {
        signal: ((input?) => void) | string,
        params? :any
    }
}

declare module "react-select" {
    export default function (obj): void
}