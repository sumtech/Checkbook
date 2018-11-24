import { Component, OnInit } from '@angular/core';

import { TransactionsService } from './transactions.service';
import { Transaction } from './transactions.model';

@Component({
    selector: 'app-transactions-list',
    templateUrl: './transactions-list.component.html',
})
export class TransactionsListComponent implements OnInit {
    /**
     * The list of transactions.
     */
    public transactions: Transaction[];

    /**
     * The constructor.
     * @constructor
     * @param transactionsService
     */
    constructor(
        private transactionsService: TransactionsService
    ) { }

    /**
     * Handle initialization fo the component.
     */
    ngOnInit(): void {
        this.transactionsService.getAll()
            .subscribe(transactions => this.transactions = transactions);
    }
}
