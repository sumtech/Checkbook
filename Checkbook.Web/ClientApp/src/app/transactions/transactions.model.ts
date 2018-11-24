import { Account } from '../accounts/accounts.model';
import { Budget } from '../budgets/budgets.model';

/**
 * Represents a transaction.
 * @class
 */
export class Transaction {
    /**
     * The unique identifier for the transaction.
     */
    id: string;

    /**
     * The date for the transaction.
     */
    date: Date;

    /**
     * The unique identifier for the "from" account.
     */
    fromAccountId: number;

    /**
     * The "from" account.
     */
    fromAccount: Account;

    /**
     * The unique identifier for the "from" account.
     */
    toAccountId: number;

    /**
     * The "to" account.
     */
    toAccount: Account;

    /**
     * The collection of items for this transaction.
     */
    items: TransactionItem[];

    /**
     * The amount transferred for the transaction.
     */
    amount: number;

    /**
     * Information about this transaction.
     */
    notes: string;

    /**
     * Indicates whether this transaction has been processed by the bank
     * account.
     */
    isProcessed: boolean;
}

/**
 * Represents an item within a transaction associated with a particular transaction.
 */
export class TransactionItem {
    /**
     * The unique identifier for the transaction item.
     */
    id: string;

    /**
     * The budget from which the funds are being taken.
     */
    budget: Budget;

    /**
     * The description for the transaction item.
     */
    description: string;

    /**
     * The amount for the item.
     */
    amount: number;
}
