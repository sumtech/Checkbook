// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a transaction between a merchant and a bank account.
    /// The transaction items will contain the amount and budget allocations.
    /// The budgets will belong to specified categories.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Gets or sets the unique identifier for this transaction.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the date for the transaction.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the unique ID for the account from which money is
        /// getting transferred.
        /// </summary>
        public long FromAccountId { get; set; }

        /// <summary>
        /// Gets or sets the account from which money is getting transferred.
        /// </summary>
        public Account FromAccount { get; set; }

        /// <summary>
        /// Gets or sets the unique ID for the account to which money is
        /// getting transferred.
        /// </summary>
        public long ToAccountId { get; set; }

        /// <summary>
        /// Gets or sets the account to which money is getting transferred.
        /// </summary>
        public Account ToAccount { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user to which this
        /// transaction belongs.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the user to which this transaction belongs.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the collection of items for this transaction.
        /// </summary>
        public virtual List<TransactionItem> Items { get; set; }

        /// <summary>
        /// Gets the total amount for this transaction, which is the sum of the
        /// amounts for each item.
        /// </summary>
        public decimal Amount
        {
            get
            {
                return this.Items.Sum(i => i.Amount);
            }
        }

        /// <summary>
        /// Gets or sets more information about the transaction.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the transaction has been
        /// processed by the bank account.
        /// </summary>
        public bool IsProcessed { get; set; }
    }
}
