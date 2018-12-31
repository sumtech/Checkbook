import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { Category } from './budgets.model';

/**
 * The service for handling category information.
 * @class
 */
@Injectable()
export class CategoriesService {
    /**
     * The base URL for the category API calls.
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
        this.baseApiUrl = baseApiUrl + 'categories/';
    }

    /**
     * Get the list of categories which are available.
     * @returns             An observable list of categories.
     */
    public getAll(): Observable<Category[]> {
        const url: string = this.baseApiUrl;
        return this.http.get<Category[]>(url);
    }

    /**
     * Get a specified category.
     * @param id            The unique ID for the category.
     * @returns             An observable category.
     */
    public get(id: number): Observable<Category> {
        const url: string = this.baseApiUrl + id;
        return this.http.get<Category>(url);
    }

    /**
     * Save a category record.
     * @param category      The category information.
     * @returns             An observable category.
     */
    public save(category: Category): Observable<Category> {
        let url: string = this.baseApiUrl;
        if (category.id) {
            url += category.id + '/';
            return this.http.put<Category>(url, category);
        } else {
            return this.http.post<Category>(url, category);
        }
    }
}
