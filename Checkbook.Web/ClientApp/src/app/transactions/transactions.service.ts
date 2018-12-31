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
        const baseApiUrl = 'http://localhost:11111/api/';
        this.baseApiUrl = baseApiUrl + 'transactions/';
    }

    /**
     * Get the list of transactions which are available.
     * @returns             An observable list of transactions.
     */
    public getAll(): Observable<Transaction[]> {
        const url: string = this.baseApiUrl;
        return this.http.get<Transaction[]>(url);
    }

    /**
     * Get a specified transaction.
     * @param id            The unique ID for the transaction.
     * @returns             An observable transaction.
     */
    public get(id: string): Observable<Transaction> {
        const url: string = this.baseApiUrl + encodeURIComponent(id) + '/';
        return this.http.get<Transaction>(url);
    }

    /**
     * Save a transaction record.
     * @param transaction   The transaction information.
     * @returns             An observable transaction.
     */
    public save(transaction: Transaction): Observable<Transaction> {
        let url: string = this.baseApiUrl;
        if (transaction.id) {
            url += transaction.id + '/';
            return this.http.put<Transaction>(url, transaction);
        } else {
            return this.http.post<Transaction>(url, transaction);
        }
    }
}
