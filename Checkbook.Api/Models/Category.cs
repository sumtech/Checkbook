// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a category to which a collection of user budgets belong.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets the unique identifier for this category.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of subcategories that belong to this
        /// category.
        /// </summary>
        public List<Subcategory> Subcategories { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user to which this
        /// category belongs.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the user to which this category belongs.
        /// </summary>
        public User User { get; set; }
    }
}
