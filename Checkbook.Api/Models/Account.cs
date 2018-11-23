// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Models
{
    /// <summary>
    /// Represents a financial account.
    /// This could either be a bank account where the user retains funds/credit
    /// or an account for a merchant.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets or sets the unique identifier for this account.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this account.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user who manages this
        /// account.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who manages this account.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is an account owned by
        /// the user. True indicates the account will be related to budgets and
        /// other features for the usre. False indicates this is a merchant or
        /// other account not being managed by the user.
        /// </summary>
        public bool IsUserAccount { get; set; }
    }
}
