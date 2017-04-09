import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { user } from "../../../../readmodels/user";

export class logonPage {
}



@inject(HttpClient)
export class logonUser {
    public email: string;
    public uId: string;

    constructor(http: HttpClient) {
        http.fetch('http://localhost:5001/api/user/getUser', JSON.stringify({ email: this.email }))
            .then(result => result.json() as Promise<user>)
            .then(data => {
                localStorage["userId"] = data.id;
                this.uId = data.id;
            });
    }
}
