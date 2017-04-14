// Should only return true within IE < 11 but not Edge
export function isIe(): boolean {
    var doc = document as any;
    return (/*@cc_on!@*/false || !!doc.documentMode);
}

const pathLambdaRegex = /function\s*\((\w)\)\s*{\s*return\s*\1[\.]?([\w\.\[\]]+);?\s*}/g;

function cerebralPathFromFunction<T, T2>(func: (model: T) => T2, args: any[]): T2 {
    pathLambdaRegex.lastIndex = 0;
    const str = func.toString().replace(/"use strict";/, '');
    let m;

    if ((m = pathLambdaRegex.exec(str)) !== null) {
        let output = m[2];

        if (args.length > 0) {
            for (let i = 0; i < args.length; i++) {
                output = output.replace(`[${i}]`, `.${args[i]}`);
            }
        } else {
            output = output.replace(/\[/g, ".").replace(/\]/g, "");
        }

        if (output && output[0] == ".") {
            output = output.substring(1);
        }

        return output as any as T2;
    }

    return null;
}

export const pathFromModel = function <T>(child: string = "") {
    return function <TProp>(func: (model: T) => TProp, ...args) {
        return (cerebralPathFromFunction<T, TProp>(func, args) + child) as any as TProp
    }
}

export const setState = function<T, TProp>(path: (input: T) => TProp, value: TProp) {
    return ({input, state}) => {
        state.set(path(input), value);
    };
}

export const setStateFrom = function<T, TProp>(path: (input: T) => TProp, value: (input: T) => TProp) {
    return ({input, state}) => {
        state.set(path(input), value(input));
    };
}

export const whenState = function<T>(path: T) {
    return ({input, state, output}) => {
        if (state.get(path) === true) {
            return output.true()
        }
        output.false()
    };
}