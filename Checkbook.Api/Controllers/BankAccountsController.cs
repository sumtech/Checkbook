// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Controllers
{
    using System.Collections.Generic;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for managing bank accounts.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BankAccountsController : ControllerBase
    {
        /// <summary>
        /// The repository for managing bank accounts.
        /// </summary>
        private readonly IBankAccountsRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BankAccountsController"/> class.
        /// </summary>
        /// <param name="repository">The repository for managing bank accounts.</param>
        public BankAccountsController(IBankAccountsRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of all bank accounts.
        /// This method is mainly used for testing now. Later we will replace this with a
        /// version accepting filter/search criteria.
        /// </summary>
        /// <returns>The list of bank accounts.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<BankAccount>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Get()
        {
            IEnumerable<BankAccount> bankAccounts;
            try
            {
                bankAccounts = this.repository.GetBankAccounts();
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the bank accounts.");
            }

            return this.Ok(bankAccounts);
        }

        /// <summary>
        /// Gets a specified bank account.
        /// </summary>
        /// <param name="id">The unique ID for the bank account.</param>
        /// <returns>The bank account.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<BankAccount>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Get(long id)
        {
            BankAccount bankAccount;
            try
            {
                bankAccount = this.repository.GetBankAccount(id);
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the bank account.");
            }

            if (bankAccount == null)
            {
                return this.NotFound();
            }

            return this.Ok(bankAccount);
        }
    }
}
