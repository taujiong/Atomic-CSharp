using System;
using System.Collections.Generic;
using System.Globalization;
using Atomic.AspNetCore.Security.Claims;
using Atomic.IdentityServer.Api.Localization;
using Atomic.Localization.EntityFrameworkCore;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Atomic.IdentityServer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAtomicAspNetCore();

            AddLocalization(services);

            AddAuthentication(services);

            AddIdentityServer(services);

            services.AddControllers()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (_, factory) =>
                        factory.Create(typeof(IdentityServerResource));
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Atomic.IdentityServer.Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var localizationOptions =
                app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Atomic.Localization.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAtomicSecurityHeaders();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddLocalization(IServiceCollection services)
        {
            services.AddSqlLocalization();

            services.AddDbContext<LocalizationDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("Localization");
                options.UseNpgsql(connectionString, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
                });
            }, ServiceLifetime.Singleton, ServiceLifetime.Singleton);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new("zh-CN"),
                    new("en-US"),
                };

                options.ApplyCurrentCultureToResponseHeaders = true;
                options.DefaultRequestCulture = new RequestCulture("zh-CN", "zh-CN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["AuthServer:Authority"];
                    options.Audience = Configuration["AuthServer:Audience"];
                    options.TokenValidationParameters.ValidIssuer = Configuration["AuthServer:Authority"];
                });

            services.Configure<ClaimMapOption>(option =>
            {
                option.UserId = JwtClaimTypes.Subject;
                option.UserName = JwtClaimTypes.PreferredUserName;
                option.Role = JwtClaimTypes.Role;
                option.Email = JwtClaimTypes.Email;
                option.AvatarUrl = JwtClaimTypes.Picture;
            });
        }

        private void AddIdentityServer(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("IdentityServer");
            services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseNpgsql(connectionString, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
                        sqlOptions.MigrationsAssembly(typeof(Startup).Assembly.GetName().FullName);
                    });
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseNpgsql(connectionString, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
                        sqlOptions.MigrationsAssembly(typeof(Startup).Assembly.GetName().FullName);
                    });
                });
        }
    }
}