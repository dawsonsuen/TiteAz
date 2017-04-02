import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class Fetchdebt {
    public debts: Debt[];

    constructor(http: HttpClient) {
        http.fetch('http://localhost:5001/api/user/debts', JSON.stringify({ email: "elijah@test.com" }))
            .then(result => result.json() as Promise<Debt[]>)
            .then(data => {
                this.debts = data;
            });
    }
}

interface Debt {
    id: string;
    billId: string;
    debitUserId: string;
    creditUserId: string;
    amount: number;
    status: string;
}
