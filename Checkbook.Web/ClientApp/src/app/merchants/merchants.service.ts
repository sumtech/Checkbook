import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { Merchant } from './merchants.model';

/**
 * The service for handling merchants information.
 * @class
 */
@Injectable()
export class MerchantsService {
    /**
     * The base URL for the merchant API calls.
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
        this.baseApiUrl = baseApiUrl + 'merchants/';
    }

    /**
     * Get the list of merchants which are available.
     * @returns             An observable list of merchants.
     */
    public getAll(): Observable<Merchant[]> {
        let url: string = this.baseApiUrl;
        return this.http.get<Merchant[]>(url);
    }

    /**
     * Get a specified merchant.
     * @param id            The unique ID for the merchant.
     * @returns             An observable merchant.
     */
    public get(id: string): Observable<Merchant> {
        let url: string = this.baseApiUrl + encodeURIComponent(id) + '/';
        return this.http.get<Merchant>(url);
    }

    /**
     * Save a merchant record.
     * @param merchant      The merchant information.
     * @returns             An observable merchant.
     */
    public save(merchant: Merchant): Observable<Merchant> {
        let url: string = this.baseApiUrl;
        if (merchant.id) {
            url += encodeURIComponent(merchant.id) + '/';
            return this.http.put<Merchant>(url, merchant);
        } else {
            return this.http.post<Merchant>(url, merchant);
        }
    }
}
