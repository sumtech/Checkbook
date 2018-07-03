// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Models
{
    /// <summary>
    /// Represents an item within a transaction associated with a particular transaction.
    /// </summary>
    public class TransactionItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for this item.
        /// </summary>
        public long Id { get; set; }
    }
}