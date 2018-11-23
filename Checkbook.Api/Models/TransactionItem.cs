// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

using System.ComponentModel;
using Newtonsoft.Json;

namespace Checkbook.Api.Models
{
    /// <summary>
    /// Represents an item within a transaction associated with a particular
    /// item or group of items associated with the same budget.
    /// </summary>
    public class TransactionItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for this item.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the transaction to which
        /// this item belongs.
        /// </summary>
        public long TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the transaction to which this item belongs.
        /// </summary>
        public Transaction Transaction { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the budget associated with
        /// this transaction item.
        /// </summary>
        public long BudgetId { get; set; }

        /// <summary>
        /// Gets or sets the budget associated with this transaction item.
        /// </summary>
        public Budget Budget { get; set; }

        /// <summary>
        /// Gets or sets a description for the item. This would typically be
        /// the description of the item(s) purchased or what the items were
        /// used for.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the amount for this item.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Amount { get; set; }
    }
}
