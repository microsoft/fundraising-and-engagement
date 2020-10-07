using System;
using FundraisingandEngagement.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentDriver.Models;
using PaymentDriver.Services;
using PaymentProcessors;

namespace PaymentDriver
{
	internal class Program
	{
		public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

		private static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host
				.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostContext, configApp) =>
				{
					var builtConfig = configApp.Build();

					if (!String.IsNullOrEmpty(builtConfig["KeyVaultName"]))
						configApp.AddAzureKeyVault($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/");
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					var instrumentationKey = hostingContext.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
					if (!String.IsNullOrEmpty(instrumentationKey))
						logging.AddApplicationInsights(instrumentationKey);
				})
				.ConfigureServices((context, services) =>
				{
					services.Configure<TestDataOption>(context.Configuration.GetSection("TestData"));

					var connectionStringBuilder = new SqlConnectionStringBuilder(context.Configuration.GetConnectionString("PaymentContext"));

					if (!String.IsNullOrEmpty(context.Configuration["ConnectionSecrets:PaymentContextUserID"]))
					{
						connectionStringBuilder.UserID = context.Configuration["ConnectionSecrets:PaymentContextUserID"];
						connectionStringBuilder.Password = context.Configuration["ConnectionSecrets:PaymentContextPassword"];
					}

					services.AddDbContext<PaymentContext>(options => options.UseSqlServer(connectionStringBuilder.ConnectionString), ServiceLifetime.Singleton);

					if (Boolean.Parse(context.Configuration["TestData:UseMock"]))
					{
						services.AddSingleton<IPaymentProcessorGateway, MockProcessPayment>();
					}
					else
					{
						services.AddPaymentProcessors();
					}

					services.AddHostedService<ExecuteService>();

					services.AddSingleton<IPaymentContext>(p => p.GetRequiredService<PaymentContext>());
					services.AddSingleton<ICreatePayment, CreatePayments>();
				})
				.UseConsoleLifetime();
		}
	}
}
