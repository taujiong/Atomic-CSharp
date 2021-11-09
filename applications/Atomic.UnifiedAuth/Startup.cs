using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Atomic.AspNetCore.Security.Claims;
using Atomic.Localization.EntityFrameworkCore;
using Atomic.UnifiedAuth.Data;
using Atomic.UnifiedAuth.Localization;
using Atomic.UnifiedAuth.Users;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Atomic.UnifiedAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAtomicAspNetCore();

            AddLocalization(services);

            AddAuthentication(services);

            AddIdentityServer(services);

            services.AddRazorPages(options =>
                {
                    options.Conventions.AuthorizePage("/Profile");
                })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (_, factory) =>
                        factory.Create(typeof(AccountResource));
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            var localizationOptions =
                app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAtomicSecurityHeaders();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
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
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("Identity");
                options.UseNpgsql(connectionString, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
                });
            });

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>,
                UserClaimsPrincipalFactory>();

            services.AddIdentity<AppUser, IdentityRole>()
                .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddGitHub(options =>
                {
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    options.ClientId = Configuration["ExternalIdentityProviders:GitHub:ClientId"];
                    options.ClientSecret = Configuration["ExternalIdentityProviders:GitHub:ClientSecret"];
                    options.ClaimActions.MapJsonKey(JwtClaimTypes.Picture, "avatar_url", "url");
                });
        }

        private void AddIdentityServer(IServiceCollection services)
        {
            services.Configure<ClaimMapOption>(option =>
            {
                option.UserId = JwtClaimTypes.Subject;
                option.UserName = JwtClaimTypes.PreferredUserName;
                option.Role = JwtClaimTypes.Role;
                option.Email = JwtClaimTypes.Email;
                option.AvatarUrl = JwtClaimTypes.Picture;
            });

            var builder = services.AddIdentityServer(options =>
                {
                    options.IssuerUri = Configuration["AuthServer:IssuerUri"];
                    options.UserInteraction.ErrorUrl = "/Error";
                })
                .AddAspNetIdentity<AppUser>();

            var connectionString = Configuration.GetConnectionString("IdentityServer");
            builder.AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseNpgsql(connectionString, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
                    });
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseNpgsql(connectionString, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
                    });
                });

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                builder.AddSigningCredential(new X509Certificate2(
                    Configuration["AuthServer:CertPath"],
                    Configuration["AuthServer:CertPassword"]
                ));
            }
        }
    }
}