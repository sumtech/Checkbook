// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for managing merchants.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MerchantsController : ControllerBase
    {
        /// <summary>
        /// The repository for managing merchants.
        /// </summary>
        private readonly IMerchantsRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantsController"/> class.
        /// </summary>
        /// <param name="repository">The repository for managing merchants.</param>
        public MerchantsController(IMerchantsRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of all merchants.
        /// This method is mainly used for testing now. Later we will replace this with a
        /// version accepting filter/search criteria.
        /// </summary>
        /// <returns>The list of merchants.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Merchant>), 200)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            IEnumerable<Merchant> merchants;
            try
            {
                merchants = this.repository.GetMerchants();
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the merchants.");
            }

            return this.Ok(merchants);
        }

        /// <summary>
        /// Gets a specified merchant.
        /// </summary>
        /// <param name="id">The unique ID for the merchant.</param>
        /// <returns>The list of merchants.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<Merchant>), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public IActionResult Get(long id)
        {
            Merchant merchant;
            try
            {
                merchant = this.repository.GetMerchant(id);
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the merchant.");
            }

            if (merchant == null)
            {
                return this.NotFound();
            }

            return this.Ok(merchant);
        }
    }
}
