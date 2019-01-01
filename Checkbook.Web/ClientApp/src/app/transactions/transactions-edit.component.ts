import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { Transaction, TransactionItem } from './transactions.model';
import { TransactionsService } from './transactions.service';

import { BankAccount, MerchantAccount } from '../accounts/accounts.model';
import { AccountsService } from '../accounts/accounts.service';

import { Budget } from '../budgets/budgets.model';
import { BudgetsService } from '../budgets/budgets.service';

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
    public merchantAccounts: MerchantAccount[];

    /**
     * the list of available budgets.
     */
    public budgets: Budget[];

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
        private accountsService: AccountsService,
        private budgetsService: BudgetsService,
        private route: ActivatedRoute,
        private router: Router,
        private transactionsService: TransactionsService
    ) { }

    /**
     * Initialize the component.
     */
    ngOnInit(): void {
        const id: string = this.route.snapshot.paramMap.get('id');
        if (id) {
            // A transaction is being edited.
            this.transactionsService.get(id)
                .subscribe((transaction) => {
                    transaction.date = new Date(transaction.date);
                    this.transaction = transaction;
                });
        } else {
            // A new transaction is being created.
            this.transaction = new Transaction();
            this.transaction.items.push({} as TransactionItem);
        }

        this.accountsService.getBankAccounts()
            .subscribe((bankAccounts) => {
                this.bankAccounts = bankAccounts;
            });

        this.accountsService.getMerchantAccounts()
            .subscribe((merchantAccounts) => {
                this.merchantAccounts = merchantAccounts;
            });

        this.budgetsService.getAll()
            .subscribe((budgets) => {
                this.budgets = budgets;
            });
    }

    /**
     * Create a new item entry to begin adding a new item.
     */
    startItem() {
        this.transaction.items.push({} as TransactionItem);
    }

    /**
     * Remove item from the transaction.
     * @param item                  The item to remove.
     */
    remove(item: TransactionItem) {
        const index: number = this.transaction.items.indexOf(item);
        this.transaction.items.splice(index, 1);
    }

    /**
     * Returns the grand total amount.
     * @returns                     The grand total amount.
     */
    grandTotal() {
        if (!this.transaction || !this.transaction.items) {
            return 0;
        }

        let total = 0;
        for (let i = 0, len: number = this.transaction.items.length; i < len; i++) {
            if (this.transaction.items[i].amount) {
                total += this.transaction.items[i].amount;
            }
        }

        return total;
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
