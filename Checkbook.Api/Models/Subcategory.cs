// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Models
{
    /// <summary>
    /// Represents a subcategory to which a budget belongs. A subcategory
    /// should represent a collection of budgets for reporting purposes.
    /// </summary>
    public class Subcategory
    {
        /// <summary>
        /// Gets or sets the unique identifier for this subcategory.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this subcategory.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the category to which this
        /// subcategory belongs.
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category to which this subcategory belongs.
        /// </summary>
        public Category Category { get; set; }
    }
}
