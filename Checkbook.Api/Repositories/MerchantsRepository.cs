// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Checkbook.Api.Models;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    /// <summary>
    /// A repository for managing merchants.
    /// </summary>
    public class MerchantsRepository : IMerchantsRepository
    {
        /// <summary>
        /// The context for communicating with the checkbook database.
        /// </summary>
        private readonly CheckbookContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantsRepository"/> class.
        /// </summary>
        /// <param name="context">The context for communicating with the checkbook database.</param>
        public MerchantsRepository(CheckbookContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the list of merchants.
        /// This is a simplistic method that gets all merchants. In the future, we will be filtering
        /// the merchants so we only get the merchants we are interested in (and allowed to see).
        /// </summary>
        /// <returns>A list of merchants.</returns>
        public IEnumerable<Merchant> GetMerchants()
        {
            return this.context.Merchants.AsEnumerable();
        }

        /// <summary>
        /// Gets a specified merchant record.
        /// </summary>
        /// <param name="id">The unique identifier for the merchant.</param>
        /// <returns>The merchant.</returns>
        public Merchant GetMerchant(long id)
        {
            return this.context.Merchants.Find(id);
        }

        /// <summary>
        /// Add a new merchant to the data store.
        /// </summary>
        /// <param name="merchant">The new merchant to add.</param>
        /// <returns>The saved merchant with the updated identifier.</returns>
        public Merchant Add(Merchant merchant)
        {
            EntityEntry<Merchant> savedMerchant = this.context.Merchants.Add(merchant);
            this.context.SaveChanges();
            return savedMerchant.Entity;
        }
    }
}
