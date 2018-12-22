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
    /// A repository for managing transactions.
    /// </summary>
    public class TransactionsRepository : ITransactionsRepository
    {
        /// <summary>
        /// The context for communicating with the checkbook database.
        /// </summary>
        private readonly CheckbookContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsRepository"/> class.
        /// </summary>
        /// <param name="context">The context for communicating with the checkbook database.</param>
        public TransactionsRepository(CheckbookContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the list of transactions.
        /// This is a simplistic method that gets all transactions. In the future, we will be filtering
        /// the transactions so we only get the transactions we are interested in (and allowed to see).
        /// </summary>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>A list of transactions.</returns>
        public IEnumerable<Transaction> GetAll(long userId)
        {
            return this.context.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .Include(t => t.Items)
                    .ThenInclude(i => i.Budget)
                        .ThenInclude(s => s.Category)
                .Where(t => t.FromAccount.UserId == null || t.FromAccount.UserId == userId)
                .Where(t => t.ToAccount.UserId == null || t.ToAccount.UserId == userId);
        }

        /// <summary>
        /// Gets a specified transaction record.
        /// </summary>
        /// <param name="id">The unique identifier for the transaction.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The transaction.</returns>
        public Transaction Get(long id, long userId)
        {
            Transaction transaction = this.context.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .Include(t => t.Items)
                    .ThenInclude(i => i.Budget)
                        .ThenInclude(s => s.Category)
                .Where(t => t.FromAccount.UserId == null || t.FromAccount.UserId == userId)
                .Where(t => t.ToAccount.UserId == null || t.ToAccount.UserId == userId)
                .SingleOrDefault(t => t.Id == id);

            if (transaction == null)
            {
                throw new NotFoundException("The transaction was not found.");
            }

            return transaction;
        }

        /// <summary>
        /// Add a new transaction to the data store.
        /// </summary>
        /// <param name="transaction">The new transaction to add.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved transaction with the updated identifier.</returns>
        public Transaction Add(Transaction transaction, long userId)
        {
            // Verify we do not have an ID set, which would indicate the Save method should have been used.
            if (transaction.Id != 0)
            {
                throw new ArgumentException("A new transaction without a specified ID should have been used. To update a transaction, use the Save method.", "transaction.Id");
            }

            // Verify the user information is valid.
            Account fromAccount = this.context.Accounts.AsNoTracking().SingleOrDefault(a => a.Id == transaction.FromAccountId);
            if (fromAccount == null || (fromAccount.IsUserAccount && fromAccount.UserId != userId))
            {
                throw new ArgumentException("The account was not found.", "transaction.FromAccountId");
            }

            Account toAccount = this.context.Accounts.AsNoTracking().SingleOrDefault(a => a.Id == transaction.ToAccountId);
            if (toAccount == null || (toAccount.IsUserAccount && toAccount.UserId != userId))
            {
                throw new ArgumentException("The account was not found.", "transaction.ToAccountId");
            }

            // Verify we have at least one user account.
            if (!fromAccount.IsUserAccount && !toAccount.IsUserAccount)
            {
                throw new ArgumentException("At least on of the accounts needs to be an account for the user.", "accounts");
            }

            // Save the new transaction.
            EntityEntry<Transaction> savedTransaction = this.context.Transactions.Add(transaction);
            this.context.SaveChanges();
            return savedTransaction.Entity;
        }

        /// <summary>
        /// Saves updates to a transaction.
        /// </summary>
        /// <param name="transaction">The transaction to be saved.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved transaction information.</returns>
        public Transaction Save(Transaction transaction, long userId)
        {
            // Verify we do have an ID set.
            if (transaction.Id == 0)
            {
                throw new ArgumentException("A new transaction with a specified ID should have been used. To add a transaction, use the Add method.", "transaction.Id");
            }

            // Verify the user information is valid.
            Account fromAccount = this.context.Accounts.AsNoTracking().SingleOrDefault(a => a.Id == transaction.FromAccountId);
            if (fromAccount == null || (fromAccount.IsUserAccount && fromAccount.UserId != userId))
            {
                throw new ArgumentException("The account was not found.", "transaction.FromAccountId");
            }

            Account toAccount = this.context.Accounts.AsNoTracking().SingleOrDefault(a => a.Id == transaction.ToAccountId);
            if (toAccount == null || (toAccount.IsUserAccount && toAccount.UserId != userId))
            {
                throw new ArgumentException("The account was not found.", "transaction.ToAccountId");
            }

            // Verify we have at least one user account.
            if (!fromAccount.IsUserAccount && !toAccount.IsUserAccount)
            {
                throw new ArgumentException("At least on of the accounts needs to be an account for the user.", "accounts");
            }

            // Verify the transaction in the database is for the current user.
            Transaction previousTransaction = this.context.Transactions.AsNoTracking()
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .SingleOrDefault(t => t.Id == transaction.Id);
            if (previousTransaction.FromAccount.IsUserAccount && previousTransaction.FromAccount.UserId != userId)
            {
                throw new ArgumentException("This transaction belongs to another user.", "accounts");
            }

            if (previousTransaction.ToAccount.IsUserAccount && previousTransaction.ToAccount.UserId != userId)
            {
                throw new ArgumentException("This transaction belongs to another user.", "accounts");
            }

            // Update the transaction as well as the transaction items.
            this.context.Entry(transaction).State = EntityState.Modified;
            foreach (TransactionItem item in transaction.Items)
            {
                if (item.Id != 0)
                {
                    this.context.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    this.context.Entry(item).State = EntityState.Added;
                }
            }

            // Delete items that were not passed back.
            List<long> savedItemIds = transaction.Items.Select(x => x.Id).ToList();
            Transaction dbTransaction = this.context.Transactions.AsNoTracking()
                .Include(t => t.Items)
                .FirstOrDefault(t => t.Id == transaction.Id);
            IEnumerable<TransactionItem> itemsToRemove = dbTransaction.Items
                .Where(i => !savedItemIds.Contains(i.Id));
            foreach (TransactionItem item in itemsToRemove)
            {
                this.context.Entry(item).State = EntityState.Deleted;
            }

            this.context.SaveChanges();

            return transaction;
        }
    }
}
