using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using UbisoftConnect.Configurations;
using Xunit;

namespace UbisoftConnect.WebAPI.Tests.Integration
{
	[CollectionDefinition("Integration")]
	public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
	{
		private readonly IHost _host;
		private readonly TestServer _server;
		private readonly HttpClient _client;

		public Request Request { get; }
		public FeedbackServiceConfiguration Configuration { get; }

		public IntegrationFixture()
		{
			Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
			var hostBuilder = new HostBuilder()
						.ConfigureWebHost(webHost =>
						{
							webHost.UseTestServer();
							webHost.UseStartup<Startup>();
						})
						.UseServiceProviderFactory(new AutofacServiceProviderFactory());

			var configBuilder = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			var configRoot = configBuilder.Build();
			Configuration = configRoot.GetSection("FeedbackServiceConfiguration").Get<FeedbackServiceConfiguration>();

			_host = hostBuilder.Start();
			_server = _host.GetTestServer();
			_client = _server.CreateClient();

			Request = new Request(_client);
		}

		public void Dispose()
		{
			_client.Dispose();
			_server.Dispose();
		}
	}
}
