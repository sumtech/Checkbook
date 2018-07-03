// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a transaction between a merchant and a bank account.
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
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the amount transferred for the transaction.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the unique ID for the merchant.
        /// </summary>
        public long MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the merchant.
        /// </summary>
        public virtual Merchant Merchant { get; set; }

        /// <summary>
        /// Gets or sets the unique ID for the bank account.
        /// </summary>
        public long BankAccountId { get; set; }

        /// <summary>
        /// Gets or sets a bank account.
        /// </summary>
        public virtual BankAccount BankAccount { get; set; }

        /// <summary>
        /// Gets or sets the collection of items for this transaction.
        /// </summary>
        public virtual List<TransactionItem> TransactionItems { get; set; }
    }
}
