// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using Checkbook.Api.Models;

    /// <summary>
    /// Represents a repository for managing merchants.
    /// </summary>
    public interface IMerchantsRepository
    {
        /// <summary>
        /// Gets the list of merchants.
        /// This is a simplistic method that gets all merchants. In the future, we will be filtering
        /// the merchants so we only get the merchants we are interested in (and allowed to see).
        /// </summary>
        /// <returns>A list of merchants.</returns>
        IEnumerable<Merchant> GetMerchants();

        /// <summary>
        /// Add a new merchant to the data store.
        /// </summary>
        /// <param name="merchant">The new merchant to add.</param>
        /// <returns>The saved merchant with the updated identifier.</returns>
        Merchant Add(Merchant merchant);

        /// <summary>
        /// Gets a specified merchant record.
        /// </summary>
        /// <param name="id">The unique identifier for the merchant.</param>
        /// <returns>The merchant.</returns>
        Merchant GetMerchant(long id);
    }
}
