using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace App
{
    public static class Program
    {
        public static async Task<int> Main()
        {
            try
            {
                IHost host = CreateHostBuilder().Build();

                await host.RunAsync()
                          .ContinueWith(_ => { })
                          .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
                return -1;
            }

            return 0;
        }

        private static IHostBuilder CreateHostBuilder() =>
            new HostBuilder()
               .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.json", false, false)
                           .AddEnvironmentVariables();

                    context.Configuration = builder.Build();
                })
               .UseDefaultServiceProvider((context, options) =>
                {
                    bool isDevelopment = context.HostingEnvironment.IsDevelopment();
                    options.ValidateScopes = isDevelopment;
                    options.ValidateOnBuild = isDevelopment;
                })
               .ConfigureLogging((_, builder) =>
                {
                    builder.ClearProviders();

                    LoggerConfiguration loggerBuilder = new LoggerConfiguration()
                                                       .Enrich.FromLogContext()
                                                       .MinimumLevel.Information();

                    loggerBuilder.WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                        theme: AnsiConsoleTheme.Literate);

                    builder.AddSerilog(loggerBuilder.CreateLogger());
                })
               .ConfigureServices((hostContext, services) =>
                {
                    string connectionString = hostContext.Configuration.GetConnectionString("Default");
                    services.AddDbContext<DataContext>(o => o.UseSqlServer(connectionString));

                    services.AddHostedService<Worker>();
                });
    }
}
