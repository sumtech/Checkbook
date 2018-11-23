// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Checkbook.Api.Models;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    /// <summary>
    /// A repository for managing accounts.
    /// </summary>
    public class AccountsRepository : IAccountsRepository
    {
        /// <summary>
        /// The context for communicating with the checkbook database.
        /// </summary>
        private readonly CheckbookContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsRepository"/> class.
        /// </summary>
        /// <param name="context">The context for communicating with the checkbook database.</param>
        public AccountsRepository(CheckbookContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the list of bank accounts.
        /// </summary>
        /// <param name="userId">The unique identifier cfor the current user.</param>
        /// <returns>A list of bank accounts.</returns>
        public IEnumerable<Account> GetBankAccounts(long userId)
        {
            return this.context.Accounts
                .Where(a => a.IsUserAccount)
                .Where(a => a.UserId == userId)
                .AsEnumerable();
        }

        /// <summary>
        /// Gest the list of merchant accounts.
        /// </summary>
        /// <returns>A list of merchant accounts.</returns>
        public IEnumerable<Account> GetMerchantAccounts()
        {
            return this.context.Accounts
                .Where(a => !a.IsUserAccount)
                .AsEnumerable();
        }

        /// <summary>
        /// Add a new account to the data store.
        /// </summary>
        /// <param name="account">The new account to add.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved account with the updated identifier.</returns>
        public Account Add(Account account, long userId)
        {
            EntityEntry<Account> savedAccount = this.context.Accounts.Add(account);
            this.context.SaveChanges();
            return savedAccount.Entity;
        }

        /// <summary>
        /// Gets a specified account record.
        /// </summary>
        /// <param name="accountId">The unique identifier for the account.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The account.</returns>
        public Account GetAccount(long accountId, long userId)
        {
            return this.context.Accounts
                .Where(a => a.UserId == userId)
                .Single(a => a.Id == accountId);
        }
    }
}
