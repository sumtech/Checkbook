import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { Transaction } from './transactions.model';
import { TransactionsService } from './transactions.service';

@Component({
    selector: 'app-transactions-edit',
    templateUrl: './transactions-edit.component.html',
})
export class TransactionsEditComponent implements OnInit {
    /**
     * transaction information.
     */
    public transaction: Transaction;

    /**
     * Constructor.
     * @constructor
     * @param route
     * @param router
     * @param service
     */
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private service: TransactionsService
    ) { }

    /**
     * Initialize the component.
     */
    ngOnInit(): void {
        const id: string = this.route.snapshot.paramMap.get('id');
        this.service.getTransaction(id)
            .subscribe(transaction => this.transaction = transaction);
    }

    /**
     * Save changes to the transaction.
     */
    onSubmit(): void {
        this.service.saveTransaction(this.transaction)
            .subscribe(() => {
                this.router.navigate(['/transactions']);
            });
    }
}
