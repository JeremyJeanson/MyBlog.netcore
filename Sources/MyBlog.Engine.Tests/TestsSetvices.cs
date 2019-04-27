using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Engine;
using MyBlog.Engine.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MyBlog.Engine.Tests
{
    public class TestsSetvices
    {
        #region Singleton

        static TestsSetvices()
        {
            Current = new TestsSetvices();
        }

        public static TestsSetvices Current { get; }

        #endregion

        #region Déclaration

        private readonly ServiceProvider _serviceProvider;

        #endregion

        #region Constructor

        private TestsSetvices()
        {
            // Build config
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            // Build service provider
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMyBlogEngine(config,null);
            _serviceProvider = serviceCollection.BuildServiceProvider();
            //try
            //{
                _serviceProvider.GetService<DataContext>().Database.Migrate();
            //}
            //catch (Exception ex)
            //{
            //    Trace.TraceError(ex.Message);
            //}
        }

        #endregion

        #region Methodes

        public T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        #endregion
    }
}
