import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Budget, Category } from './budgets.model';
import { BudgetsService } from './budgets.service';
import { CategoriesService } from './categories.service';

@Component({
    selector: 'app-budgets-edit',
    templateUrl: './budgets-edit.component.html',
})
export class BudgetsEditComponent implements OnInit {
    /**
     * Budget information.
     */
    public budget: Budget;

    /**
     * The list of available categories.
     */
    public categories: Category[];

    /**
     * Constructor.
     * @constructor
     * @param budgetsService
     * @param categoriesService
     * @param route
     * @param router
     */
    constructor(
        private budgetsService: BudgetsService,
        private categoriesService: CategoriesService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    /**
     * Initialize the component.
     */
    ngOnInit(): void {
        const id: number = Number(this.route.snapshot.paramMap.get('id'));
        if (id) {
            // A budget is being edited.
            this.budgetsService.get(id)
                .subscribe((budget) => {
                    this.budget = budget;
                });
        } else {
            // A new budget is being created.
            this.budget = new Budget();
        }

        this.categoriesService.getAll()
            .subscribe((categories) => {
                this.categories = categories;
            });
    }

    /**
     * Save changes to the budget.
     */
    onSubmit(): void {
        this.budgetsService.save(this.budget)
            .subscribe(() => {
                this.router.navigate(['/budgets']);
            });
    }
}
