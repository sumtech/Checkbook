namespace Checkbook.Api.Models
{
    using System;

    /// <summary>
    /// Represents a transaction between a merchant and a bank account.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Gets or sets the unique identifier for this transaction.
        /// </summary>
        public Guid Id { get; set; }
    }
}
