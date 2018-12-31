// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api
{
    using System;
    using System.Collections.Generic;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Microsoft.Data.Sqlite;
    using System.Data.Common;

    /// <summary>
    /// The startup configuration methods.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration options.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration infrormation used to startup the application.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Adds services to the container.
        /// This method gets called by the runtime.
        /// </summary>
        /// <param name="services">The services to add to the container.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add CORS policies.
            services.AddCors(options => options.AddPolicy("AnyOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddCors(options => options.AddPolicy("LocalOrigin", builder =>
            {
                builder
                    .WithOrigins("http://localhost:22222")
                    ////.WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(opts => opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddJsonOptions(opts => opts.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore);

            // Register application services.
            ////services.AddDbContext<CheckbookContext>(opt => opt.UseInMemoryDatabase("Checkbook"));

            // Open a connection to a SQLite database.
            string connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = ":memory:",
            }.ToString();
            DbConnection dbConnection = new SqliteConnection(connectionString);
            services.AddDbContext<CheckbookContext>(opt => opt.UseSqlite(dbConnection));

            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<IBudgetsRepository, BudgetsRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
        }

        /// <summary>
        /// Configures the HTTP request pipeline.
        /// This method gets called by the runtime.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment information.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Handle exceptions on our local machines.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use the CORS policy.
            app.UseCors("LocalOrigin");

            // Use the framework services.
            app.UseMvc();

            using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
            {
                CheckbookContext context = serviceScope.ServiceProvider.GetService<CheckbookContext>();
                this.AddTestData(context);
            }
        }

        /// <summary>
        /// Adds test data to a context.
        /// </summary>
        /// <param name="context">The context to which data will be added.</param>
        private void AddTestData(CheckbookContext context)
        {
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            // Users
            User user1 = new User
            {
                Id = 1,
                Username = "Dude",
                FirstName = "Brian",
                LastName = "Dorgan",
            };

            context.Users.Add(user1);

            // Accounts
            Account bankAccount1 = new Account
            {
                Id = 1,
                Name = "My Account",
                IsUserAccount = true,
                UserId = user1.Id,
            };

            context.Accounts.Add(bankAccount1);

            Account merchantAccount1 = new Account
            {
                Id = 2,
                Name = "Awesome Company",
                IsUserAccount = false,
            };
            Account merchantAccount2 = new Account
            {
                Id = 3,
                Name = "Wonderful Restaurant",
                IsUserAccount = false,
            };
            Account merchantAccount3 = new Account
            {
                Id = 4,
                Name = "Apple and Suasage Factory",
                IsUserAccount = false,
            };

            context.Accounts.Add(merchantAccount1);
            context.Accounts.Add(merchantAccount2);
            context.Accounts.Add(merchantAccount3);

            // Categories
            Category categoryFood = new Category
            {
                Id = 1,
                UserId = user1.Id,
                Name = "Food",
            };
            Category categoryEntertainment = new Category
            {
                Id = 2,
                UserId = user1.Id,
                Name = "Entertainment",
            };
            Category categoryTransportation = new Category
            {
                Id = 3,
                UserId = user1.Id,
                Name = "Transportation",
            };

            context.Categories.Add(categoryFood);
            context.Categories.Add(categoryEntertainment);
            context.Categories.Add(categoryTransportation);

            // Budgets.
            Budget budgetGroceries = new Budget
            {
                Id = 1,
                UserId = user1.Id,
                Name = "Groceries",
                CategoryId = categoryFood.Id,
            };
            Budget budgetPeaches = new Budget
            {
                Id = 2,
                UserId = user1.Id,
                Name = "Peaches",
                CategoryId = categoryFood.Id,
            };
            Budget budgetRestaruants = new Budget
            {
                Id = 3,
                UserId = user1.Id,
                Name = "Restaurants",
                CategoryId = categoryFood.Id,
            };
            Budget budgetDates = new Budget
            {
                Id = 4,
                UserId = user1.Id,
                Name = "Dates",
                CategoryId = categoryEntertainment.Id,
            };
            Budget budgetVacations = new Budget
            {
                Id = 5,
                UserId = user1.Id,
                Name = "Vacations",
                CategoryId = categoryEntertainment.Id,
            };
            Budget budgetGasoline = new Budget
            {
                Id = 6,
                UserId = user1.Id,
                Name = "Gasoline",
                CategoryId = categoryTransportation.Id,
            };
            Budget budgetCarInsurance = new Budget
            {
                Id = 7,
                UserId = user1.Id,
                Name = "Car Insurance",
                CategoryId = categoryTransportation.Id,
            };
            Budget budgetOilChanges = new Budget
            {
                Id = 8,
                UserId = user1.Id,
                Name = "Oil Changes",
                CategoryId = categoryTransportation.Id,
            };
            Budget budgetHawaii = new Budget
            {
                Id = 9,
                UserId = user1.Id,
                Name = "Hawaii",
                CategoryId = categoryEntertainment.Id,
            };
            Budget budgetNewCar = new Budget
            {
                Id = 10,
                UserId = user1.Id,
                Name = "New Car",
                CategoryId = categoryTransportation.Id,
            };

            context.Budgets.Add(budgetGroceries);
            context.Budgets.Add(budgetPeaches);
            context.Budgets.Add(budgetRestaruants);
            context.Budgets.Add(budgetDates);
            context.Budgets.Add(budgetVacations);
            context.Budgets.Add(budgetGasoline);
            context.Budgets.Add(budgetCarInsurance);
            context.Budgets.Add(budgetOilChanges);
            context.Budgets.Add(budgetHawaii);
            context.Budgets.Add(budgetNewCar);

            // Transactions
            Transaction transaction1 = new Transaction
            {
                Id = 1,
                Date = DateTime.Now.AddDays(-7),
                FromAccountId = bankAccount1.Id,
                UserId = user1.Id,
                ToAccountId = merchantAccount1.Id,
                Items = new List<TransactionItem>
                {
                    new TransactionItem
                    {
                        Id = 1,
                        TransactionId = 1,
                        BudgetId = budgetOilChanges.Id,
                        Amount = 50.00m,
                    },
                },
            };
            Transaction transaction2 = new Transaction
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-5),
                FromAccountId = bankAccount1.Id,
                ToAccountId = merchantAccount2.Id,
                UserId = user1.Id,
                Items = new List<TransactionItem>
                {
                    new TransactionItem
                    {
                        Id = 2,
                        TransactionId = 2,
                        BudgetId = budgetGroceries.Id,
                        Amount = 25.00m,
                    },
                },
            };
            Transaction transaction3 = new Transaction
            {
                Id = 3,
                Date = DateTime.Now.AddDays(-2),
                FromAccountId = bankAccount1.Id,
                ToAccountId = merchantAccount3.Id,
                UserId = user1.Id,
                Items = new List<TransactionItem>
                {
                    new TransactionItem
                    {
                        Id = 3,
                        TransactionId = 3,
                        BudgetId = budgetDates.Id,
                        Amount = 100.00m,
                    },
                    new TransactionItem
                    {
                        Id = 4,
                        TransactionId = budgetRestaruants.Id,
                        BudgetId = 2,
                        Amount = 200.00m,
                    },
                    new TransactionItem
                    {
                        Id = 5,
                        TransactionId = 3,
                        BudgetId = budgetHawaii.Id,
                        Amount = 300.00m,
                    },
                },
            };

            context.Transactions.Add(transaction1);
            context.Transactions.Add(transaction2);
            context.Transactions.Add(transaction3);

            context.SaveChanges();
        }
    }
}
