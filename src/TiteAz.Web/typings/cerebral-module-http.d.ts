declare module "cerebral-module-http" {
    interface HttpModuleOptions {
        method?: string,
        baseUrl?: string,
        headers?: { [id: string]: string },
        onRequest?: (xhr: XMLHttpRequest, options: HttpResponseOptions) => void;
        onResponse?: (response: XMLHttpRequest, resolve: Function, reject: Function) => void;
    }

    interface HttpRequestOptions {
        query?: { [id: string]: any },
    }

    interface HttpResponseOptions extends HttpRequestOptions, HttpModuleOptions {
        body: string
    }

    interface HttpRequestReponse<T> {
        result: T,
        status: string
    }

    interface HttpModule {
        request: any,
        get<T>(url: string, options?: HttpRequestOptions): Promise<HttpRequestReponse<T>>,
        post<T>(url: string, body: any, options?: HttpRequestOptions): Promise<HttpRequestReponse<T>>,
        put<T>(url: string, body: any, options?: HttpRequestOptions): Promise<HttpRequestReponse<T>>,
        patch<T>(url: string, body: any, options?: HttpRequestOptions): Promise<HttpRequestReponse<T>>,
        delete<T>(url: string, options?: HttpRequestOptions): Promise<HttpRequestReponse<T>>,
        fileUpload<T>(options: FileUploadOptions): FileUpload<T>,
        updateOptions(newOptions: HttpModuleOptions): void;
        abort(regexp: string): boolean;
    }

    export default function (moduleOptions: HttpModuleOptions): HttpModule;

    interface FileUploadProgress {
        progress: number
    }

    interface FileUploadOptions {
        url: string,
        name?: string,
        data?: { [id: string]: any },
        headers?: { [id: string]: string },
        onProgress?: (progress: FileUploadProgress) => void
    }

    interface FileUploadResult<T> {
        result?: T,
        status: string
    }

    class FileUpload<TResponse> {
        constructor(options: FileUploadOptions);
        xhr: XMLHttpRequest;
        isAborted: boolean;
        send: (files: FileList | File[]) => Promise<FileUploadResult<TResponse>>;
        abort: () => void;
    }
}