import { Component, OnInit } from '@angular/core';

import { AccountsService } from './accounts.service';
import { MerchantAccount } from './accounts.model';

@Component({
    selector: 'app-merchant-accounts-list',
    templateUrl: './merchant-accounts-list.component.html',
})
export class MerchantAccountsListComponent implements OnInit {
    /**
     * The list of accounts.
     */
    public accounts: MerchantAccount[];

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
        this.accountsService.getMerchantAccounts()
            .subscribe(accounts => this.accounts = accounts);
    }
}
