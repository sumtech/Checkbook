// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for managing transactions.
    /// </summary>
    [Produces("application/json")]
    public class TransactionsController : ControllerBase
    {
        /// <summary>
        /// The repository for managing transactions.
        /// </summary>
        private readonly ITransactionsRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsController"/> class.
        /// </summary>
        /// <param name="repository">The repository for managing transactions.</param>
        public TransactionsController(ITransactionsRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of all transactions.
        /// This method is mainly used for testing now. Later we will replace this with a
        /// version accepting filter/search criteria.
        /// </summary>
        /// <returns>The list of transactions.</returns>
        [HttpGet("api/transactions")]
        [ProducesResponseType(typeof(List<Transaction>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Get()
        {
            IEnumerable<Transaction> transactions;
            try
            {
                transactions = this.repository.GetTransactions();
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the transactions.");
            }

            if (transactions == null)
            {
                return this.Ok(new List<Transaction>());
            }

            return this.Ok(transactions);
        }

        /// <summary>
        /// Gets a specified transaction.
        /// </summary>
        /// <param name="id">The unique ID for the transaction.</param>
        /// <returns>The list of transactions.</returns>
        [HttpGet("api/transactions/{id:long}")]
        [ProducesResponseType(typeof(List<Transaction>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Get(long id)
        {
            Transaction transaction;
            try
            {
                transaction = this.repository.GetTransaction(id);
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the transaction.");
            }

            if (transaction == null)
            {
                return this.NotFound();
            }

            return this.Ok(transaction);
        }

        /// <summary>
        /// Updates a transaction.
        /// </summary>
        /// <param name="id">The unique ID for the transaction.</param>
        /// <param name="transaction">The transaction information.</param>
        /// <returns>The updated transaction.</returns>
        [HttpPut("api/transactions/{id:long}")]
        [ProducesResponseType(typeof(List<Transaction>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Put(long id, [FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return this.StatusCode(500, "A transaction must be passed in for it to be saved.");
            }

            if (transaction.Id != id)
            {
                return this.StatusCode(500, "The transaction ID values did not match.");
            }

            Transaction savedTransaction;
            try
            {
                savedTransaction = this.repository.SaveTransaction(transaction);
            }
            catch
            {
                return this.StatusCode(500, "There was an error saving the transaction.");
            }

            return this.Ok(transaction);
        }
    }
}
