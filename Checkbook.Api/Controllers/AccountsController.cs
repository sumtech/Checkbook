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
        [HttpGet("api/accounts/{accountId:long}")]
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

        /// <summary>
        /// Adds an account.
        /// </summary>
        /// <param name="account">The account information.</param>
        /// <returns>The saved account.</returns>
        [HttpPost("api/accounts")]
        [ProducesResponseType(typeof(List<Account>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Post([FromBody] Account account)
        {
            long userId = 1;

            if (account == null)
            {
                return this.BadRequest("An account must be passed in for it to be saved.");
            }

            if (account.IsUserAccount)
            {
                if (account.UserId == null || account.UserId == 0)
                {
                    account.UserId = userId;
                }
            }

            Account savedAccount;
            try
            {
                savedAccount = this.accountsRepository.Add(account, userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error saving the account.");
            }

            return this.Ok(savedAccount);
        }

        /// <summary>
        /// Updates an account.
        /// </summary>
        /// <param name="accountId">The unique ID for the account.</param>
        /// <param name="account">The account information.</param>
        /// <returns>The updated account.</returns>
        [HttpPut("api/accounts/{accountId:long}")]
        [ProducesResponseType(typeof(List<Account>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Put(long accountId, [FromBody] Account account)
        {
            long userId = 1;

            if (account == null)
            {
                return this.BadRequest("An account must be passed in for it to be saved.");
            }

            if (account.Id != accountId)
            {
                return this.BadRequest("The account ID values did not match.");
            }

            if (account.IsUserAccount)
            {
                if (account.UserId == null || account.UserId == 0)
                {
                    account.UserId = userId;
                }
            }

            Account savedAccount;
            try
            {
                savedAccount = this.accountsRepository.Save(account, userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error saving the account.");
            }

            return this.Ok(savedAccount);
        }
    }
}
