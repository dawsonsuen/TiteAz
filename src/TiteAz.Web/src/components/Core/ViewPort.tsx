import * as React from 'react';
import { Views } from "models/ViewsStateModel";
import { getPath } from "models/StateModel";

export class ViewPort extends React.Component<{ view: Views | Views[] }, {}> {
    render() {
        var comp = [];
        var set = false;
        React.Children.forEach(this.props.children, x => {
            var match = Array.isArray((x as any).props.match) ? (x as any as View).props.match : [(x as any).props.match]
                if (Array.isArray(this.props.view)) {
                    this.props.view.forEach(y => {
                        if ((match as any).indexOf(y) >= 0) {
                            comp.push(x);
                        }
                    })
                }
                if ((match as any).indexOf(this.props.view) >= 0) {
                    comp.push(x);
                }
        });
        return comp.length == 1 ? comp[0] as React.ReactElement<any> : <div> {comp} </div>;
    }
}

export class View extends React.Component<{ match: Views | Views[] }, {}> {
    render() {
        if (React.Children.count(this.props.children) == 0)
            return null;
        if (typeof this.props.children === 'string')
            return <span>{this.props.children}</span>
        if (React.Children.count(this.props.children) > 1)
            return <div> {this.props.children} </div>
        return this.props.children as React.ReactElement<any>;
    }
}