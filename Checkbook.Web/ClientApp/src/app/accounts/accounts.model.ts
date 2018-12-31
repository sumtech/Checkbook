/**
 * Represents an account.
 * @class
 */
export class Account {
    /**
     * Constructor.
     * @constructor
     */
    constructor() {
        this.id = 0;
    }

    /**
     * The unique identifier for the account.
     */
    id: number;

    /**
     * The name of the account.
     */
    name: string;

    /**
     * Indicates whether this is a user account ("my" account).
     */
    isUserAccount: boolean;
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
