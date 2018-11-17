// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using Checkbook.Api.Models;

    /// <summary>
    /// Represents a repository for managing bank accounts.
    /// </summary>
    public interface IBankAccountsRepository
    {
        /// <summary>
        /// Gets the list of bank accounts.
        /// This is a simplistic method that gets all bank accounts. In the future, we will be filtering
        /// the bank accounts so we only get the bank accounts we are interested in (and allowed to see).
        /// </summary>
        /// <returns>A list of bank accounts.</returns>
        IEnumerable<BankAccount> GetBankAccounts();

        /// <summary>
        /// Add a new bank account to the data store.
        /// </summary>
        /// <param name="bankAccount">The new bank account to add.</param>
        /// <returns>The saved bank account with the updated identifier.</returns>
        BankAccount Add(BankAccount bankAccount);

        /// <summary>
        /// Gets a specified bank account record.
        /// </summary>
        /// <param name="id">The unique identifier for the bank account.</param>
        /// <returns>The bank account.</returns>
        BankAccount GetBankAccount(long id);
    }
}
