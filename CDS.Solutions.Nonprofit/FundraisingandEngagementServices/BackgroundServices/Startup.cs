using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FundraisingandEngagement.BackgroundServices.DataPush;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Options;
using FundraisingandEngagement.Services.Xrm;

namespace FundraisingandEngagement.BackgroundServices
{
	public static class Startup
	{
		public static IHostBuilder ConfigureHostBuilder(this IHostBuilder hostBuilder, ILogger? log)
		{
			return hostBuilder
				.ConfigureAppConfiguration((hostContext, configApp) =>
				{
					var builtConfig = configApp
							.SetBasePath(Path.GetDirectoryName(typeof(Program).Assembly.Location))
							.Build();

					if (!String.IsNullOrEmpty(builtConfig["KeyVaultName"]))
						configApp.AddAzureKeyVault($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/");
				})
				.ConfigureServices((context, services) =>
				{
					services.Configure<CrmOptions>(context.Configuration.GetSection("Crm"));

					var connectionStringBuilder = new SqlConnectionStringBuilder(context.Configuration.GetConnectionString("PaymentContext"))
					{
						UserID = context.Configuration["ConnectionSecrets:PaymentContextUserID"],
						Password = context.Configuration["ConnectionSecrets:PaymentContextPassword"]
					};

					services.AddDbContext<PaymentContext>(options => options.UseSqlServer(connectionStringBuilder.ConnectionString), ServiceLifetime.Singleton);
					services.AddSingleton<ILogger>(provider => log ?? provider.GetRequiredService<ILoggerFactory>().CreateLogger("BackgroundServices"));
					services.AddHttpClient();

					services.AddSingleton<IPaymentContext>(provider => provider.GetRequiredService<PaymentContext>());
					services.AddSingleton<IXrmService, XrmService>();
					services.AddSingleton<EntitiesSequenceBackgroundService>();
					services.AddSingleton<DataPushService>();
					services.AddSingleton<Functions>();
					services.AddSingleton<YearlyGivingFromEntity>();
					services.AddSingleton<YearlyGivingFull>();
					services.AddSingleton<PerformanceCalculation>();
					services.AddSingleton<EventReceipting>();
				});
		}
	}
}
