// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Checkbook.Api.Models;
    using Microsoft.EntityFrameworkCore;
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
        /// Gets a specified account record.
        /// </summary>
        /// <param name="accountId">The unique identifier for the account.</param>
        /// <param name="userId">The unique identifier for the current user.
        /// Bank accounts require a matching user ID. Merchant accounts will
        /// match if the user ID is either 0 or null.</param>
        /// <returns>The account.</returns>
        public Account Get(long accountId, long? userId)
        {
            Account account = this.context.Accounts.SingleOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                throw new NotFoundException("The account was not found.");
            }

            if (account.IsUserAccount)
            {
                // We have a bank account.
                // Verify the correct user ID was passed in.
                if (userId == null || userId == 0)
                {
                    throw new ArgumentException("A user ID is expected to be passed in for a bank account.", "userId");
                }

                if (account.UserId != userId)
                {
                    throw new ArgumentException("The user ID did not match for the bank account.", "userId");
                }
            }

            return account;
        }

        /// <summary>
        /// Add a new account to the data store.
        /// </summary>
        /// <param name="account">The new account to add.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved account with the updated identifier.</returns>
        public Account Add(Account account, long? userId)
        {
            // Verify we do not have an ID set, which would indicate the Save method should have been used.
            if (account.Id != 0)
            {
                throw new ArgumentException("A new account without a specified ID should have been used. To update an account, use the Save method.", "account.Id");
            }

            // Verify we have a user ID.
            if (userId == null || userId == 0)
            {
                throw new ArgumentException("A user ID is expected to be passed in for an account.", "userId");
            }

            if (account.IsUserAccount)
            {
                // We have a bank account.
                // Verify the correct user ID was passed in.
                if (account.UserId == null || account.UserId == 0)
                {
                    throw new ArgumentException("A bank account is expected to have a user ID.", "account.UserId");
                }

                if (account.UserId != userId)
                {
                    throw new ArgumentException("A user ID is expected to match the passed in user ID for a bank account.", "account.UserId");
                }
            }
            else
            {
                // We have a merchant account.
                // Verify no valid user ID was passed in.
                if (account.UserId != null && account.UserId != 0)
                {
                    throw new ArgumentException("A merchant account is not expected to have a user ID.", "account.UserId");
                }
            }

            // Save the new transaction.
            EntityEntry<Account> savedAccount = this.context.Accounts.Add(account);
            this.context.SaveChanges();
            return savedAccount.Entity;
        }

        /// <summary>
        /// Saves updates to an account.
        /// </summary>
        /// <param name="account">The account to be saved.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved account information.</returns>
        public Account Save(Account account, long? userId)
        {
            // Verify we do have an ID set.
            if (account.Id == 0)
            {
                throw new ArgumentException("A new account with a specified ID should have been used. To add an account, use the Add method.", "account.Id");
            }

            // Verify we have a user ID.
            if (userId == null || userId == 0)
            {
                throw new ArgumentException("A user ID is expected to be passed in for an account.", "userId");
            }

            if (account.IsUserAccount)
            {
                // We have a bank account.
                // Verify the correct user ID was passed in.
                if (account.UserId == null || account.UserId == 0)
                {
                    throw new ArgumentException("A bank account is expected to have a user ID.", "account.UserId");
                }

                if (account.UserId != userId)
                {
                    throw new ArgumentException("A user ID is expected to match the passed in user ID for a bank account.", "account.UserId");
                }
            }
            else
            {
                // We have a merchant account.
                // Verify no valid user ID was passed in.
                if (account.UserId != null && account.UserId != 0)
                {
                    throw new ArgumentException("A merchant account is not expected to have a user ID.", "account.UserId");
                }
            }

            // Update the account.
            this.context.Entry(account).State = EntityState.Modified;
            this.context.SaveChanges();

            return account;
        }
    }
}
