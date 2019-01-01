import { Component, OnInit } from '@angular/core';

import { BudgetsService } from './budgets.service';
import { Budget, Category } from './budgets.model';

@Component({
    selector: 'app-budgets-list',
    templateUrl: './budgets-list.component.html',
    styleUrls: ['./budgets.component.scss'],
})
export class BudgetsListComponent implements OnInit {
    /**
     * The list of budgets.
     */
    public budgets: Budget[];

    /**
     * The list of categories.
     */
    public categories: Category[] = [];

    /**
     * The columns to display in the list.
     */
    public columnsToDisplay: string[] = ['id', 'name', 'balance', 'actions'];

    /**
     * The constructor.
     * @constructor
     * @param budgetsService
     */
    constructor(
        private budgetsService: BudgetsService
    ) { }

    /**
     * Handle initialization fo the component.
     */
    ngOnInit(): void {
        this.budgetsService.getAllWithTotals()
            .subscribe(budgets => {
                this.budgets = budgets;

                let category: Category | undefined;
                for (let i = 0, len: number = budgets.length; i < len; i++) {
                    if (!category || budgets[i].categoryId !== category.id) {
                        category = budgets[i].category;
                        this.categories.push(category);
                    }

                    category.budgets.push(budgets[i]);
                }
            });
    }
}
