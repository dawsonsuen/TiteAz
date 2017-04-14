declare module "cerebral-module-router" {
    interface RoutesConfig {
        [route: string]: string
    }

    interface RouterOptions {
        baseUrl?: string
        query?: boolean
        onlyHash?: boolean
        preventAutostart?: boolean
        allowEscape?: boolean
    }

    interface RedirectOptions {
        replace: boolean
    }

    interface RouterModule {
        detach(): void
        getUrl(): string
        getSignalUrl(signalName: string, payload?: any): string | void
        redirect(url: string, options?: RedirectOptions): void
        redirectToSignal(signalName: string, payload?: any): void
        trigger(url?: string): void
    }

    export default function (routesConfig: RoutesConfig, options?: RouterOptions): RouterModule;
}

declare module "cerebral-module-router/redirect" {
    interface RedirectOptions {
        replace: boolean
    }
    export default function (url: string, options?: RedirectOptions): any;
}