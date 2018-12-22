// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System.Collections.Generic;
    using Checkbook.Api.Models;

    /// <summary>
    /// Represents a repository for managing transactions.
    /// </summary>
    public interface ITransactionsRepository
    {
        /// <summary>
        /// Gets the list of transactions.
        /// This is a simplistic method that gets all transactions. In the future, we will be filtering
        /// the transactions so we only get the transactions we are interested in (and allowed to see).
        /// </summary>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>A list of transactions.</returns>
        IEnumerable<Transaction> GetAll(long userId);

        /// <summary>
        /// Add a new transaction to the data store.
        /// </summary>
        /// <param name="transaction">The new transaction to add.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved transaction with the updated identifier.</returns>
        Transaction Add(Transaction transaction, long userId);

        /// <summary>
        /// Gets a specified transaction record.
        /// </summary>
        /// <param name="id">The unique identifier for the transaction.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The transaction.</returns>
        Transaction Get(long id, long userId);

        /// <summary>
        /// Saves updates for a transaction.
        /// </summary>
        /// <param name="transaction">The transaction to be saved.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved transaction information.</returns>
        Transaction Save(Transaction transaction, long userId);
    }
}
