import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { Transaction } from './transactions.model';

/**
 * The service for handling transactions information.
 * @class
 */
@Injectable()
export class TransactionsService {
    /**
     * The base URL for the transaction API calls.
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
        let baseApiUrl = 'http://localhost:11111/api/';
        this.baseApiUrl = baseApiUrl + 'transactions/';
    }

    /**
     * Get the list of transactions which are available.
     * @returns             An observable list of transactions.
     */
    public getTransactions(): Observable<Transaction[]> {
        let url: string = this.baseApiUrl;
        return this.http.get<Transaction[]>(url);
    }

    /**
     * Get a specified transactions.
     * @param id            The unique ID for the transactions.
     * @returns             An observable transactions.
     */
    public getTransaction(id: string): Observable<Transaction> {
        let url: string = this.baseApiUrl + encodeURIComponent(id) + '/';
        return this.http.get<Transaction>(url);
    }

    /**
     * Save a transaction record.
     * @param transaction   The transactions information.
     * @returns             An observable transactions.
     */
    public saveTransaction(transaction: Transaction): Observable<Transaction> {
        let url: string = this.baseApiUrl;
        if (transaction.id) {
            url += encodeURIComponent(transaction.id) + '/';
            return this.http.put<Transaction>(url, transaction);
        } else {
            return this.http.post<Transaction>(url, transaction);
        }
    }
}
