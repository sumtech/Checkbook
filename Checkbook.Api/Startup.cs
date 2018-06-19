// Copyright (c) Palouse Coding Congolmeration. All Rights Reserved.

namespace Checkbook.Api
{
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
        }
    }
}
