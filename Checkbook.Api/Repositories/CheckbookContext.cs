namespace Checkbook.Api.Repositories
{
    using Checkbook.Api.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The context used for communicating with the checkbook database.
    /// </summary>
    public class CheckbookContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckbookContext"/> class.
        /// </summary>
        /// <param name="options">The options for setting up the context.</param>
        public CheckbookContext(DbContextOptions<CheckbookContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the database set used to manage the transactions.
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }
    }
}
