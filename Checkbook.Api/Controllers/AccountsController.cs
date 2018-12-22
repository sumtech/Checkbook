// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Controllers
{
    using System.Collections.Generic;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for managing accounts.
    /// </summary>
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        /// <summary>
        /// The repository for managing accounts.
        /// </summary>
        private readonly IAccountsRepository accountsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsController"/> class.
        /// </summary>
        /// <param name="accountsRepository">The repository for managing accounts.</param>
        public AccountsController(IAccountsRepository accountsRepository)
        {
            this.accountsRepository = accountsRepository;
        }

        /// <summary>
        /// Gets the list of all bank accounts.
        /// </summary>
        /// <returns>The list of bank accounts.</returns>
        [HttpGet("api/accounts/my-accounts")]
        [ProducesResponseType(typeof(List<Account>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult GetBankAccounts()
        {
            long userId = 1;

            IEnumerable<Account> bankAccounts;
            try
            {
                bankAccounts = this.accountsRepository.GetBankAccounts(userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the bank accounts.");
            }

            if (bankAccounts == null)
            {
                return this.Ok(new List<Account>());
            }

            return this.Ok(bankAccounts);
        }

        /// <summary>
        /// Gets the list of all merchant accounts.
        /// </summary>
        /// <returns>The list of merchant accounts.</returns>
        [HttpGet("api/accounts/merchants")]
        [ProducesResponseType(typeof(List<Account>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult GetMerchantAccounts()
        {
            IEnumerable<Account> merchantAccounts;
            try
            {
                merchantAccounts = this.accountsRepository.GetMerchantAccounts();
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the merchant accounts.");
            }

            if (merchantAccounts == null)
            {
                return this.Ok(new List<Account>());
            }

            return this.Ok(merchantAccounts);
        }

        /// <summary>
        /// Gets a specified account.
        /// </summary>
        /// <param name="accountId">The unique ID for the account.</param>
        /// <returns>The account.</returns>
        [HttpGet("api/accounts/{id:long}")]
        [ProducesResponseType(typeof(List<Account>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Get(long accountId)
        {
            long userId = 1;

            Account account;
            try
            {
                account = this.accountsRepository.Get(accountId, userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the account.");
            }

            if (account == null)
            {
                return this.NotFound();
            }

            return this.Ok(account);
        }
    }
}
