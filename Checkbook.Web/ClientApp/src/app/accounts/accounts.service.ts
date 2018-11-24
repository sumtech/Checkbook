import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { Account, BankAccount, MerchantAccount } from './accounts.model';

/**
 * The service for handling account information.
 * @class
 */
@Injectable()
export class AccountsService {
    /**
     * The base URL for the account API calls.
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
        this.baseApiUrl = baseApiUrl + 'accounts/';
    }

    /**
     * Get the list of bank accounts which are available.
     * @returns             An observable list of bank accounts.
     */
    public getBankAccounts(): Observable<BankAccount[]> {
        let url: string = this.baseApiUrl + 'my-accounts/';
        return this.http.get<BankAccount[]>(url);
    }

    /**
     * Get the list of merchant accounts which are available.
     * @returns             An observable list of merchant accounts.
     */
    public getMerchantAccounts(): Observable<MerchantAccount[]> {
        let url: string = this.baseApiUrl + 'merchants/';
        return this.http.get<MerchantAccount[]>(url);
    }

    /**
     * Get a specified account.
     * @param id            The unique ID for the account.
     * @returns             An observable account.
     */
    public get(id: string): Observable<Account> {
        let url: string = this.baseApiUrl + encodeURIComponent(id) + '/';
        return this.http.get<Account>(url);
    }

    /**
     * Save an account record.
     * @param account       The account information.
     * @returns             An observable account.
     */
    public save(account: Account): Observable<Account> {
        let url: string = this.baseApiUrl;
        if (account.id) {
            url += encodeURIComponent(account.id) + '/';
            return this.http.put<Account>(url, account);
        } else {
            return this.http.post<Account>(url, account);
        }
    }
}
