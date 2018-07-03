// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Models
{
    /// <summary>
    /// Represents a bank account in which the user accumulates funds.
    /// </summary>
    public class BankAccount
    {
        /// <summary>
        /// Gets or sets the unique identifier for this bank account.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this account.
        /// </summary>
        public string Name { get; set; }
    }
}