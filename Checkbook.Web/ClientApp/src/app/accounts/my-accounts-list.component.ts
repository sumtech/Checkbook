import { Component, OnInit } from '@angular/core';

import { AccountsService } from './accounts.service';
import { BankAccount } from './accounts.model';

@Component({
    selector: 'app-my-accounts-list',
    templateUrl: './my-accounts-list.component.html',
})
export class MyAccountsListComponent implements OnInit {
    /**
     * The list of accounts.
     */
    public accounts: BankAccount[];

    /**
     * The columns to display in the list.
     */
    public columnsToDisplay: string[] = ['id', 'name', 'actions'];

    /**
     * The constructor.
     * @constructor
     * @param accountsService
     */
    constructor(
        private accountsService: AccountsService
    ) { }

    /**
     * Handle initialization fo the component.
     */
    ngOnInit(): void {
        this.accountsService.getBankAccounts()
            .subscribe(accounts => this.accounts = accounts);
    }
}
