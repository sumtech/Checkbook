// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System;
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
        public virtual DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the database set used to manage the accounts.
        /// </summary>
        public virtual DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the database set used to manage budgets.
        /// </summary>
        public virtual DbSet<Budget> Budgets { get; set; }

        /// <summary>
        /// Gets or sets the database set used to manage categories.
        /// </summary>
        public virtual DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the database set used to manage budgets.
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// Configures the model from the entity types exposed in DbSet properties on the derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>().HasOne(m => m.FromAccount)
                .WithMany().HasForeignKey(m => m.FromAccountId);

            modelBuilder.Entity<Transaction>().HasOne(m => m.ToAccount)
                .WithMany().HasForeignKey(m => m.ToAccountId);
        }
    }
}
