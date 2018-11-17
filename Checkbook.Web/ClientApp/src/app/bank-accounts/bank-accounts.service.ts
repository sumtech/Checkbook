import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { BankAccount } from './bank-accounts.model';

/**
 * The service for handling bank account information.
 * @class
 */
@Injectable()
export class BankAccountsService {
    /**
     * The base URL for the bank account API calls.
     */
    private baseApiUrl;

    /**
     * The constructor.
     * @constructor
     * @param http
     */
    constructor(
        private http: HttpClient,
    ) {
        let baseApiUrl: string = 'http://localhost:11111/api/';
        this.baseApiUrl = baseApiUrl + 'bankaccounts/';
    }

    /**
     * Get the list of bank accounts which are available.
     * @returns             An observable list of bank accounts.
     */
    public getAll(): Observable<BankAccount[]> {
        let url: string = this.baseApiUrl;
        return this.http.get<BankAccount[]>(url);
    }

    /**
     * Get a specified bank account.
     * @param id            The unique ID for the bank account.
     * @returns             An observable bank account.
     */
    public get(id: string): Observable<BankAccount> {
        let url: string = this.baseApiUrl + encodeURIComponent(id) + '/';
        return this.http.get<BankAccount>(url);
    }

    /**
     * Save a bank account record.
     * @param bankAccount   The bank account information.
     * @returns             An observable bank account.
     */
    public save(bankAccount: BankAccount): Observable<BankAccount> {
        let url: string = this.baseApiUrl;
        if (bankAccount.id) {
            url += encodeURIComponent(bankAccount.id) + '/';
            return this.http.put<BankAccount>(url, bankAccount);
        } else {
            return this.http.post<BankAccount>(url, bankAccount);
        }
    }
}
