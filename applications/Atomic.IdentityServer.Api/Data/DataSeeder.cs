using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Atomic.IdentityServer.Api.Data
{
    public static class DataSeeder
    {
        public static void SeedData(ConfigurationDbContext context, IConfiguration configuration)
        {
            context.Database.EnsureCreated();

            if (!context.IdentityResources.Any())
            {
                AddIdentityResources(context);
            }

            if (!context.Clients.Any())
            {
                AddDeveloperClient(context, configuration);
            }
        }

        private static void AddIdentityResources(ConfigurationDbContext context)
        {
            var identityResources = new IdentityResource[]
            {
                new IdentityResources.Profile(),
                new IdentityResources.OpenId(),
            };

            foreach (var identityResource in identityResources)
            {
                context.IdentityResources.Add(identityResource.ToEntity());
            }

            context.SaveChanges();
        }

        private static void AddDeveloperClient(ConfigurationDbContext context, IConfiguration configuration)
        {
            var clientName = configuration["InitialClient:Name"];
            var clientDescription = configuration["InitialClient:Description"];
            var clientSecret = configuration["InitialClient:Secret"];

            var client = new Client
            {
                ClientId = clientName,
                ClientName = clientName,
                Description = clientDescription,
                ClientSecrets = { new Secret(clientSecret.Sha256()) },
                AllowedGrantTypes = { "password" },
                AllowedScopes =
                {
                    "IdentityServer.Client.Get",
                    "IdentityServer.Client.Create",
                    "IdentityServer.ApiResource.Get",
                    "IdentityServer.ApiResource.Create",
                },
            };

            context.Clients.Add(client.ToEntity());
            context.SaveChanges();
        }
    }
}