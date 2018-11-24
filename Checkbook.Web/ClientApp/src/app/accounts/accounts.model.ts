/**
 * Represents an account.
 * @class
 */
export class Account {
    /**
     * The unique identifier for the account.
     */
    id: string;

    /**
     * The name of the account.
     */
    name: string;
}

/**
 * Represents a bank account.
 * @class
 */
export class BankAccount extends Account {
}

/**
 * Represents a merchant.
 * @class
 */
export class MerchantAccount extends Account {
}
