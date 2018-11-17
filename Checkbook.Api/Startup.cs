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
            services.AddMvc();

            // Register application services.
            services.AddDbContext<CheckbookContext>(opt => opt.UseInMemoryDatabase("Checkbook"));

            services.AddScoped<IBankAccountsRepository, BankAccountsRepository>();
            services.AddScoped<IMerchantsRepository, MerchantsRepository>();
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

        private void AddTestData(CheckbookContext context)
        {
            context.Merchants.Add(new Merchant
            {
                Id = 1,
                Name = "Awesome Company",
            });
            context.Merchants.Add(new Merchant
            {
                Id = 2,
                Name = "Wonderful Restaurant",
            });
            context.Merchants.Add(new Merchant
            {
                Id = 3,
                Name = "Apple and Suasage Factory",
            });

            context.Transactions.Add(new Transaction
            {
                Id = 1,
                TransactionDate = DateTime.Now.AddDays(-7),
                Amount = 50.00m,
                MerchantId = 1,
                Merchant = new Merchant
                {
                    Id = 1,
                    Name = "Awesome Company",
                },
                BankAccountId = 1,
                BankAccount = new BankAccount
                {
                    Id = 1,
                    Name = "My Account",
                },
                TransactionItems = new List<TransactionItem>(),
            });
            context.Transactions.Add(new Transaction
            {
                Id = 2,
                TransactionDate = DateTime.Now.AddDays(-5),
                Amount = 25.00m,
                MerchantId = 2,
                Merchant = new Merchant
                {
                    Id = 2,
                    Name = "Wonderful Restaurant",
                },
                BankAccountId = 1,
                BankAccount = new BankAccount
                {
                    Id = 1,
                    Name = "My Account",
                },
                TransactionItems = new List<TransactionItem>(),
            });
            context.Transactions.Add(new Transaction
            {
                Id = 3,
                TransactionDate = DateTime.Now.AddDays(-2),
                Amount = 500.00m,
                MerchantId = 3,
                Merchant = new Merchant
                {
                    Id = 3,
                    Name = "Apple and Suasage Factory",
                },
                BankAccountId = 1,
                BankAccount = new BankAccount
                {
                    Id = 1,
                    Name = "My Account",
                },
                TransactionItems = new List<TransactionItem>(),
            });

            context.SaveChanges();
        }
    }
}
