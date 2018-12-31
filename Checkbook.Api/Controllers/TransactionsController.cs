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
            long userId = 1;

            IEnumerable<Transaction> transactions;
            try
            {
                transactions = this.repository.GetAll(userId);
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
        /// <param name="transactionId">The unique ID for the transaction.</param>
        /// <returns>The list of transactions.</returns>
        [HttpGet("api/transactions/{transactionId:long}")]
        [ProducesResponseType(typeof(List<Transaction>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Get(long transactionId)
        {
            long userId = 1;

            Transaction transaction;
            try
            {
                transaction = this.repository.Get(transactionId, userId);
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
        /// Adds a transaction.
        /// </summary>
        /// <param name="transaction">The transaction information.</param>
        /// <returns>The saved transaction.</returns>
        [HttpPost("api/transactions")]
        [ProducesResponseType(typeof(List<Transaction>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Post([FromBody] Transaction transaction)
        {
            long userId = 1;

            if (transaction == null)
            {
                return this.BadRequest("A transaction must be passed in for it to be saved.");
            }

            transaction.UserId = userId;

            Transaction savedTransaction;
            try
            {
                savedTransaction = this.repository.Add(transaction, userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error saving the transaction.");
            }

            return this.Ok(savedTransaction);
        }

        /// <summary>
        /// Updates a transaction.
        /// </summary>
        /// <param name="transactionId">The unique ID for the transaction.</param>
        /// <param name="transaction">The transaction information.</param>
        /// <returns>The updated transaction.</returns>
        [HttpPut("api/transactions/{transactionId:long}")]
        [ProducesResponseType(typeof(List<Transaction>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Put(long transactionId, [FromBody] Transaction transaction)
        {
            long userId = 1;

            if (transaction == null)
            {
                return this.BadRequest("A transaction must be passed in for it to be saved.");
            }

            if (transaction.Id != transactionId)
            {
                return this.BadRequest("The transaction ID values did not match.");
            }

            transaction.UserId = userId;

            Transaction savedTransaction;
            try
            {
                savedTransaction = this.repository.Save(transaction, userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error saving the transaction.");
            }

            return this.Ok(savedTransaction);
        }
    }
}
