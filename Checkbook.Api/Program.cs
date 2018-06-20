// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// The main initializer of the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Starts the main application.
        /// </summary>
        /// <param name="args">The arguments used when starting the application.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Initializes the web host builder.
        /// </summary>
        /// <param name="args">The arguments used when starting the application.</param>
        /// <returns>The web host builder.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
