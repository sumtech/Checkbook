import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { Budget, Category } from './budgets.model';

/**
 * The service for handling budget information.
 * @class
 */
@Injectable()
export class BudgetsService {
    /**
     * The base URL for the budget API calls.
     */
    private baseApiUrl: string;

    /**
     * The constructor.
     * @constructor
     * @param http
     */
    constructor(
        private http: HttpClient,
    ) {
        const baseApiUrl = 'http://localhost:11111/api/';
        this.baseApiUrl = baseApiUrl + 'budgets/';
    }

    /**
     * Get the list of budgets which are available.
     * @returns             An observable list of budgets.
     */
    public getAll(): Observable<Budget[]> {
        const url: string = this.baseApiUrl;
        return this.http.get<Budget[]>(url);
    }

    /**
     * Get a specified budget.
     * @param id            The unique ID for the budget.
     * @returns             An observable budget.
     */
    public get(id: number): Observable<Budget> {
        const url: string = this.baseApiUrl + id;
        return this.http.get<Budget>(url);
    }

    /**
     * Save a budget record.
     * @param budget        The budget information.
     * @returns             An observable budget.
     */
    public save(budget: Budget): Observable<Budget> {
        let url: string = this.baseApiUrl;
        if (budget.id) {
            url += budget.id + '/';
            return this.http.put<Budget>(url, budget);
        } else {
            return this.http.post<Budget>(url, budget);
        }
    }
}
