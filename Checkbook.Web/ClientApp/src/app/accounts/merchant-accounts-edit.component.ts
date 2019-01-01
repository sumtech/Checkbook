import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { MerchantAccount } from './accounts.model';
import { AccountsService } from './accounts.service';

@Component({
    selector: 'app-merchant-accounts-edit',
    templateUrl: './merchant-accounts-edit.component.html',
})
export class MerchantAccountsEditComponent implements OnInit {
    /**
     * Account information.
     */
    public account: MerchantAccount;

    /**
     * Constructor.
     * @constructor
     * @param accountsService
     * @param route
     * @param router
     */
    constructor(
        private accountsService: AccountsService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    /**
     * Initialize the component.
     */
    ngOnInit(): void {
        const id: string = this.route.snapshot.paramMap.get('id');
        if (id) {
            // An account is being edited.
            this.accountsService.get(id)
                .subscribe((account) => {
                    this.account = account;
                });
        } else {
            // A new account is being created.
            this.account = new MerchantAccount();
        }
    }

    /**
     * Save changes to the account.
     */
    onSubmit(): void {
        this.account.isUserAccount = false;
        this.accountsService.save(this.account)
            .subscribe(() => {
                this.router.navigate(['/merchants']);
            });
    }
}
