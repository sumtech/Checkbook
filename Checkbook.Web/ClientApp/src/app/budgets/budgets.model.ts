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

    /**
     * The category to which this budget belongs.
     */
    category: Category;

    /**
     * The unique identifier for the category to which this budget belongs.
     */
    categoryId: number;
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
        this.budgets = [];
    }

    /**
     * The unique identifier for the category.
     */
    id: number;

    /**
     * The name of the category.
     */
    name: string;

    /**
     * The budgets that belong to this category.
     */
    budgets: Budget[];
}
