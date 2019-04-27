using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Engine.Data;
using MyBlog.Engine.Midleware;
using MyBlog.Engine.Models;
using System;
using System.Globalization;
using System.Linq;

namespace MyBlog.Engine
{
    public static class Extensions
    {
        /// <summary>
        /// Name of the connectionstring use to access to SQL database
        /// </summary>
        private const String ConnectionString = "Database";
        private const string CultureEnUs = "en-US";
        private const string CutulreFrFr = "fr-FR";

        public static IServiceCollection AddMyBlogEngine(this IServiceCollection services, IConfiguration configuration, IMvcCoreBuilder mvc)
        {
            // Get settings for next steps
            var settings = configuration.Get<Settings>();

            services.AddLocalization();

            // Add Controllers to the MVC pipeline
            mvc?.AddApplicationPart(typeof(Extensions).Assembly)
                .AddMvcLocalization();

            // Options
            services.Configure<Settings>(configuration);

            // Data context
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(ConnectionString)));
            //services.AddScoped<DataContext>(_ =>
            //    new DataContext(configuration.GetConnectionString(ConnectionString)));

            // HTTP Accessor
            services.AddHttpContextAccessor();

            // Session
            services.AddSession();

            #region Authentication

            // activate the authentication
            var authBuilder = services.AddAuthentication(Constants.SignInScheme)
                .AddCookie(Constants.SignInScheme,options=>
                {
                    options.LoginPath = "/Authentication";
                });

            // Microsoft authentication
            if (settings.MicrosoftAccountAuthentication.Active)
            {
                authBuilder = authBuilder.AddMicrosoftAccount(options =>
                {
                    options.SignInScheme = Constants.SignInScheme;
                    options.ClientId = settings.MicrosoftAccountAuthentication.ClientId;
                    options.ClientSecret = settings.MicrosoftAccountAuthentication.ClientSecret;
                });
            }

            // Facebook authentication
            if (settings.FacebookAuthentication.Active)
            {
                authBuilder = authBuilder.AddFacebook(options =>
                {
                    options.SignInScheme = Constants.SignInScheme;
                    options.ClientId = settings.FacebookAuthentication.ClientId;
                    options.ClientSecret = settings.FacebookAuthentication.ClientSecret;
                });
            }

            // Twitter authentication
            if (settings.TwitterAuthentication.Active)
            {
                authBuilder = authBuilder.AddTwitter(options =>
                {
                    options.SignInScheme = Constants.SignInScheme;
                    options.ConsumerKey = settings.TwitterAuthentication.ClientId;
                    options.ConsumerSecret = settings.TwitterAuthentication.ClientSecret;
                });
            }

            // Google authentication
            if (settings.GoogleAuthentication.Active)
            {
                authBuilder = authBuilder.AddGoogle(options =>
                {
                    options.SignInScheme = Constants.SignInScheme;
                    options.ClientId = settings.GoogleAuthentication.ClientId;
                    options.ClientSecret = settings.GoogleAuthentication.ClientSecret;
                });
            }

            #endregion

            #region Services

            // DataService
            services.AddScoped<DataService>();

            // Feeds
            services.AddScoped<FeedService>();
            // Files service
            services.AddScoped<FilesService>();
            // Mails
            services.AddScoped<MailService>();
            // User service
            services.AddScoped<UserService>();
            // Users settings
            services.AddScoped<UserSettingsService>();
            
            #endregion

            #region ViewModels and models

            // Layout ViewModel
            services.AddScoped<LayoutViewModel>();

            #endregion

            #region Metaweblog

            services.AddScoped<MetaWeblogService>();

            #endregion

            return services;
        }

        public static IApplicationBuilder UseMyBlogEngine(this IApplicationBuilder app)
        {
            // Cultures
            CultureInfo[] cultures = new[] {
                new CultureInfo(CultureEnUs),
                new CultureInfo(CutulreFrFr)
            };

            // Set Localization options
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(CultureEnUs),
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            });

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Initialize the database 
                if (!AllMigrationsApplied(scope.ServiceProvider.GetService<DataContext>()))
                {
                    scope.ServiceProvider.GetService<DataContext>().Database.Migrate();
                }

                // Init the storage
                scope.ServiceProvider.GetService<FilesService>().Initilize();
            }

            // Session
            app.UseSession();

            // Authentication
            app.UseAuthentication();

            // XmlRpc + Metaweblog
            app.Map("/metaweblog", MetaWeblog.Configure);

            return app;
        }


        private static Boolean AllMigrationsApplied(DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }
    }
}
