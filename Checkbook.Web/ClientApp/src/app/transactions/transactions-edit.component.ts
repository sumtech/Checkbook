import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { Transaction } from './transactions.model';
import { TransactionsService } from './transactions.service';

import { BankAccount } from '../bank-accounts/bank-accounts.model';
import { BankAccountsService } from '../bank-accounts/bank-accounts.service';

import { Merchant } from '../merchants/merchants.model';
import { MerchantsService } from '../merchants/merchants.service';

@Component({
    selector: 'app-transactions-edit',
    templateUrl: './transactions-edit.component.html',
})
export class TransactionsEditComponent implements OnInit {
    /**
     * Transaction information.
     */
    public transaction: Transaction;

    /**
     * The list of available bank accounts.
     */
    public bankAccounts: BankAccount[];

    /**
     * The list of available merchants.
     */
    public merchants: Merchant[];

    /**
     * Constructor.
     * @constructor
     * @param bankAccountsService
     * @param merchantsService
     * @param route
     * @param router
     * @param transactionsService
     */
    constructor(
        private bankAccountsService: BankAccountsService,
        private merchantsService: MerchantsService,
        private route: ActivatedRoute,
        private router: Router,
        private transactionsService: TransactionsService
    ) { }

    /**
     * Initialize the component.
     */
    ngOnInit(): void {
        const id: string = this.route.snapshot.paramMap.get('id');
        this.transactionsService.get(id)
            .subscribe((transaction) => {
                transaction.transactionDate = new Date(transaction.transactionDate);
                this.transaction = transaction;
            });

        this.bankAccountsService.getAll()
            .subscribe((bankAccounts) => {
                this.bankAccounts = bankAccounts;
            });

        this.merchantsService.getAll()
            .subscribe((merchants) => {
                this.merchants = merchants;
            });
    }

    /**
     * Save changes to the transaction.
     */
    onSubmit(): void {
        this.transactionsService.save(this.transaction)
            .subscribe(() => {
                this.router.navigate(['/transactions']);
            });
    }
}
