// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Models
{
    /// <summary>
    /// Represents a merchant account to which a user gives money for something of value.
    /// </summary>
    public class Merchant
    {
        /// <summary>
        /// Gets or sets the unique identifier for this merchant.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this merchant.
        /// </summary>
        public string Name { get; set; }
    }
}
