import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatCheckboxModule, MatDatepickerModule, MatNativeDateModule, MatInputModule } from '@angular/material';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

import { TransactionsListComponent } from './transactions/transactions-list.component';
import { TransactionsEditComponent } from './transactions/transactions-edit.component';
import { TransactionsService } from './transactions/transactions.service';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,

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

        RouterModule.forRoot([
            {
                path: '',
                component: HomeComponent,
                pathMatch: 'full'
            },

            {
                path: 'transactions',
                component: TransactionsListComponent,
                pathMatch: 'full'
            },
            {
                path: 'transactions/new',
                component: TransactionsEditComponent,
                pathMatch: 'full'
            },
            {
                path: 'transactions/edit/:id',
                component: TransactionsEditComponent,
                pathMatch: 'full'
            }
        ])
    ],
    providers: [
        TransactionsService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
