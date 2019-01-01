import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
    MatButtonModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatOptionModule,
    MatSelectModule,
    MatTableModule,
    MatToolbarModule
} from '@angular/material';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

import { AccountsService } from './accounts/accounts.service';
import { MerchantAccountsListComponent } from './accounts/merchant-accounts-list.component';
import { MerchantAccountsEditComponent } from './accounts/merchant-accounts-edit.component';
import { MyAccountsListComponent } from './accounts/my-accounts-list.component';
import { MyAccountsEditComponent } from './accounts/my-accounts-edit.component';

import { BudgetsService } from './budgets/budgets.service';
import { BudgetsListComponent } from './budgets/budgets-list.component';
import { BudgetsEditComponent } from './budgets/budgets-edit.component';

import { CategoriesService } from './budgets/categories.service';
import { CategoriesEditComponent } from './budgets/categories-edit.component';

import { TransactionsListComponent } from './transactions/transactions-list.component';
import { TransactionsEditComponent } from './transactions/transactions-edit.component';
import { TransactionsService } from './transactions/transactions.service';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,

        MerchantAccountsListComponent,
        MerchantAccountsEditComponent,
        MyAccountsListComponent,
        MyAccountsEditComponent,

        BudgetsListComponent,
        BudgetsEditComponent,

        CategoriesEditComponent,

        TransactionsListComponent,
        TransactionsEditComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,

        BrowserAnimationsModule,
        MatButtonModule,
        MatCheckboxModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatInputModule,
        MatOptionModule,
        MatSelectModule,
        MatTableModule,
        MatToolbarModule,

        RouterModule.forRoot([
            {
                path: '',
                component: HomeComponent,
                pathMatch: 'full',
            },

            {
                path: 'merchants',
                component: MerchantAccountsListComponent,
                pathMatch: 'full',
            },
            {
                path: 'merchants/new',
                component: MerchantAccountsEditComponent,
                pathMatch: 'full',
            },
            {
                path: 'merchants/edit/:id',
                component: MerchantAccountsEditComponent,
                pathMatch: 'full',
            },

            {
                path: 'my-accounts',
                component: MyAccountsListComponent,
                pathMatch: 'full',
            },
            {
                path: 'my-accounts/new',
                component: MyAccountsEditComponent,
                pathMatch: 'full',
            },
            {
                path: 'my-accounts/edit/:id',
                component: MyAccountsEditComponent,
                pathMatch: 'full',
            },

            {
                path: 'budgets',
                component: BudgetsListComponent,
                pathMatch: 'full',
            },
            {
                path: 'budgets/new',
                component: BudgetsEditComponent,
                pathMatch: 'full',
            },
            {
                path: 'budgets/edit/:id',
                component: BudgetsEditComponent,
                pathMatch: 'full',
            },

            {
                path: 'categories/new',
                component: CategoriesEditComponent,
                pathMatch: 'full',
            },
            {
                path: 'categories/edit/:id',
                component: CategoriesEditComponent,
                pathMatch: 'full',
            },

            {
                path: 'transactions',
                component: TransactionsListComponent,
                pathMatch: 'full',
            },
            {
                path: 'transactions/new',
                component: TransactionsEditComponent,
                pathMatch: 'full',
            },
            {
                path: 'transactions/edit/:id',
                component: TransactionsEditComponent,
                pathMatch: 'full',
            },
        ])
    ],
    providers: [
        AccountsService,
        BudgetsService,
        CategoriesService,
        TransactionsService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
