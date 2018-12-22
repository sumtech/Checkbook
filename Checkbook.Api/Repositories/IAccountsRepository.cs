// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using Checkbook.Api.Models;

    /// <summary>
    /// Represents a repository for managing financial accounts.
    /// </summary>
    public interface IAccountsRepository
    {
        /// <summary>
        /// Gets the list of bank accounts.
        /// </summary>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>A list of bank accounts.</returns>
        IEnumerable<Account> GetBankAccounts(long userId);

        /// <summary>
        /// Gest the list of merchant accounts.
        /// </summary>
        /// <returns>A list of merchant accounts.</returns>
        IEnumerable<Account> GetMerchantAccounts();

        /// <summary>
        /// Gets a specified account record.
        /// </summary>
        /// <param name="accountId">The unique identifier for the account.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The account.</returns>
        Account Get(long accountId, long? userId);

        /// <summary>
        /// Add a new account to the data store.
        /// </summary>
        /// <param name="account">The new account to add.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved account with the updated identifier.</returns>
        Account Add(Account account, long? userId);
    }
}
