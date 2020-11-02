using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TaskSubject.API;
using TaskSubject.DataAccess.Infrastructure;

namespace TaskSubject.IntegrationTest
{
    /// <summary>
    /// Setup to one time per session because right now local cosmos Db emulator doesn't work,
    /// and create/remove DB for every test has no sense 
    /// </summary>
    public abstract class IntegrationWithDataBaseTest: IntegrationTest
    {
        private WebApplicationFactory<Startup> _appFactory;

        [OneTimeSetUp]
        public override void SetupOnTime()
        {
            _appFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                builder.ConfigureAppConfiguration(config => config.AddUserSecrets<Program>());
            });

            try
            {
                SeedData.DiscourageContainer(_appFactory.Services);
            }
            catch
            {
                // ignored
            }

            SeedData.Initialize(_appFactory.Services);
            Client = _appFactory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDownOnTime()
        {
            SeedData.DiscourageContainer(_appFactory.Services);
        }
    }


    public abstract class IntegrationTest
    {
        protected HttpClient Client;
        private WebApplicationFactory<Startup> _appFactory;

        [OneTimeSetUp]
        public virtual void SetupOnTime()
        {
            _appFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                builder.ConfigureAppConfiguration(config => config.AddUserSecrets<Program>());
            });

            Client = _appFactory.CreateClient();
        }
    }
}
