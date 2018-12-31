import { Component, OnInit } from '@angular/core';

import { BudgetsService } from './budgets.service';
import { Budget } from './budgets.model';

@Component({
    selector: 'app-budgets-list',
    templateUrl: './budgets-list.component.html',
})
export class BudgetsListComponent implements OnInit {
    /**
     * The list of budgets.
     */
    public budgets: Budget[];

    /**
     * The columns to display in the list.
     */
    public columnsToDisplay: string[] = ['id', 'category', 'name', 'actions'];

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
        this.budgetsService.getAll()
            .subscribe(budgets => this.budgets = budgets);
    }
}
