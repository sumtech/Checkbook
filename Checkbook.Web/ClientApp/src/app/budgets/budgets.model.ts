/**
 * Represents a budget.
 * @class
 */
export class Budget {
    /**
     * Constructor.
     * @constructor
     */
    constructor() {
        this.id = 0;
    }

    /**
     * The unique identifier for the budget.
     */
    id: number;

    /**
     * The name of the budget.
     */
    name: string;
}

/**
 * Represetns a category to which a budget may belong.
 */
export class Category {
    /**
     * Constructor.
     * @constructor
     */
    constructor() {
        this.id = 0;
    }
    
    /**
     * The unique identifier for the category.
     */
    id: number;

    /**
     * The name of the category.
     */
    name: string;
}
