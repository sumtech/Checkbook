// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Checkbook.Api.Models;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    /// <summary>
    /// A repository for managing bankAccounts.
    /// </summary>
    public class BankAccountsRepository : IBankAccountsRepository
    {
        /// <summary>
        /// The context for communicating with the checkbook database.
        /// </summary>
        private readonly CheckbookContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BankAccountsRepository"/> class.
        /// </summary>
        /// <param name="context">The context for communicating with the checkbook database.</param>
        public BankAccountsRepository(CheckbookContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the list of bank accounts.
        /// This is a simplistic method that gets all bank accounts. In the future, we will be filtering
        /// the bank accounts so we only get the bank accounts we are interested in (and allowed to see).
        /// </summary>
        /// <returns>A list of bank accounts.</returns>
        public IEnumerable<BankAccount> GetBankAccounts()
        {
            return this.context.BankAccounts.AsEnumerable();
        }

        /// <summary>
        /// Gets a specified bank account record.
        /// </summary>
        /// <param name="id">The unique identifier for the bank account.</param>
        /// <returns>The bank account.</returns>
        public BankAccount GetBankAccount(long id)
        {
            return this.context.BankAccounts.Find(id);
        }

        /// <summary>
        /// Add a new bank account to the data store.
        /// </summary>
        /// <param name="bankAccount">The new bank account to add.</param>
        /// <returns>The saved bank account with the updated identifier.</returns>
        public BankAccount Add(BankAccount bankAccount)
        {
            EntityEntry<BankAccount> savedBankAccount = this.context.BankAccounts.Add(bankAccount);
            this.context.SaveChanges();
            return savedBankAccount.Entity;
        }
    }
}
