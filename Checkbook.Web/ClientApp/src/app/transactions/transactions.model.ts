import { BankAccount } from '../bank-accounts/bank-accounts.model';
import { Merchant } from '../merchants/merchants.model';

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
    transactionDate: Date;

    /**
     * The amount transferred for the transaction.
     */
    amount: number;

    /**
     * The merchant.
     */
    merchant: Merchant;

    /**
     * The bank account.
     */
    bankAccount: BankAccount;

    /**
     * The collection of items for this transaction.
     */
    transactionItems: TransactionItem[];
}

/**
 * Represents an item within a transaction associated with a particular transaction.
 */
export class TransactionItem {
    /**
     * The unique identifier for the transaction item.
     */
    id: string;
}
