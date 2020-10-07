using System;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.RecurringDonations.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentProcessors;

namespace FundraisingandEngagement.RecurringDonations
{
	public class Program
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
					var builderConnectionStringBuilder = new SqlConnectionStringBuilder(context.Configuration.GetConnectionString("PaymentContext"))
					{
						UserID = context.Configuration["ConnectionSecrets:PaymentContextUserID"],
						Password = context.Configuration["ConnectionSecrets:PaymentContextPassword"]
					};

					var connectionString = builderConnectionStringBuilder.ConnectionString;

					services.AddDbContext<PaymentContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Singleton);
					services.AddPaymentProcessors();
					services.AddHostedService<RecurringDonationBackgroundService>();

					services.AddSingleton<IPaymentContext>(p => p.GetRequiredService<PaymentContext>());
					services.AddSingleton<IPaymentRepoService, PaymentRepoService>();
				})
				.UseConsoleLifetime();
		}
	}
}