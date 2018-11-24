// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
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
        /// <returns>A list of transactions.</returns>
        public IEnumerable<Transaction> GetTransactions()
        {
            return this.context.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .Include(t => t.Items)
                .ThenInclude(i => i.Budget)
                .ThenInclude(s => s.Category)
                .AsEnumerable();
        }

        /// <summary>
        /// Gets a specified transaction record.
        /// </summary>
        /// <param name="id">The unique identifier for the transaction.</param>
        /// <returns>The transaction.</returns>
        public Transaction GetTransaction(long id)
        {
            return this.context.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .Include(t => t.Items)
                .ThenInclude(i => i.Budget)
                .ThenInclude(s => s.Category)
                .FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Add a new transaction to the data store.
        /// </summary>
        /// <param name="transaction">The new transaction to add.</param>
        /// <returns>The saved transaction with the updated identifier.</returns>
        public Transaction Add(Transaction transaction)
        {
            EntityEntry<Transaction> savedTransaction = this.context.Transactions.Add(transaction);
            this.context.SaveChanges();
            return savedTransaction.Entity;
        }

        /// <summary>
        /// Saves updates to a transaction.
        /// </summary>
        /// <param name="transaction">The transaction to be saved.</param>
        /// <returns>The saved transaction information.</returns>
        public Transaction SaveTransaction(Transaction transaction)
        {
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
