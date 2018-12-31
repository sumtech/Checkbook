import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Category } from './budgets.model';
import { CategoriesService } from './categories.service';

@Component({
    selector: 'app-categories-edit',
    templateUrl: './categories-edit.component.html',
})
export class CategoriesEditComponent implements OnInit {
    /**
     * Category information.
     */
    public category: Category;

    /**
     * Constructor.
     * @constructor
     * @param categoriesService
     * @param route
     * @param router
     */
    constructor(
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
            // A category is being edited.
            this.categoriesService.get(id)
                .subscribe((category) => {
                    this.category = category;
                });
        } else {
            // A new category is being created.
            this.category = new Category();
        }
    }

    /**
     * Save changes to the category.
     */
    onSubmit(): void {
        this.categoriesService.save(this.category)
            .subscribe(() => {
                this.router.navigate(['/budgets']);
            });
    }
}
