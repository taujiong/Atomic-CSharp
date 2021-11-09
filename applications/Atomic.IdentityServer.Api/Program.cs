using System;
using Atomic.IdentityServer.Api.Data;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Atomic.IdentityServer.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            const string appName = "Atomic.IdentityServer";

            Log.Logger = CreateLogger();

            try
            {
                Log.Information("Configuring web host ({AppName})...", appName);
                var host = CreateHostBuilder(args).Build();

                CreateDbIfNotExists(host);

                Log.Information("Starting web host ({AppName})...", appName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Web host ({AppName}) terminated unexpectedly!", appName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static ILogger CreateLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Code)
                .CreateLogger();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ConfigurationDbContext>();
            var configuration = services.GetRequiredService<IConfiguration>();
            DataSeeder.SeedData(context, configuration);
        }
    }
}