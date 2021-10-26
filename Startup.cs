using System;
using System.Collections.Generic;
using System.Globalization;
using Localization.SqlLocalizer.DbStringLocalizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AddSqlLocalization(services);

            services.AddRazorPages()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var localizationOptions =
                app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        /// <summary>
        /// 添加基于数据库的本地化
        /// 此方法必须在 AddViewLocalization 之前执行，否则会注入默认的 ResourceManagerStringLocalizer
        /// </summary>
        private void AddSqlLocalization(IServiceCollection services)
        {
            services.AddDbContext<LocalizationModelContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("Localization");
                options.UseNpgsql(connectionString, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
                });
            }, ServiceLifetime.Singleton, ServiceLifetime.Singleton);

            services.AddSqlLocalization(options =>
            {
                options.UseTypeFullNames = true;
                options.CreateNewRecordWhenLocalisedStringDoesNotExist = true;
            });

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
    }
}