using System;
using System.Threading;
using System.Threading.Tasks;
using Data.DataSynchronization;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Data.DataSynchronization;
using FundraisingandEngagement.DataverseSynchronization;
using FundraisingandEngagement.Models.Options;
using FundraisingandEngagement.RecurringDonations.Services;
using FundraisingandEngagement.Services;
using FundraisingandEngagement.Services.DataPush;
using FundraisingandEngagement.Services.Xrm;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using PaymentProcessors;

namespace FundraisingandEngagement.RecurringDonations
{
    public class Program
    {
        static async Task Main(string[] args) {
            var host = CreateHostBuilder(args).Build();
            using (host)
            {
                await host.RunAsync();
            }
        }

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
                    logging.AddConsole();

                    var instrumentationKey = hostingContext.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                    if (!String.IsNullOrEmpty(instrumentationKey))
                        logging.AddApplicationInsights(instrumentationKey);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<CrmOptions>(options =>
                    {
                        context.Configuration.GetSection("Crm").Bind(options);
                        options.CrmApplicationKey = context.Configuration["ConnectionSecrets:DataverseClientSecret"];
                    });

                    var builderConnectionStringBuilder = new SqlConnectionStringBuilder(context.Configuration.GetConnectionString("PaymentContext"))
                    {
                        UserID = context.Configuration["ConnectionSecrets:PaymentContextUserID"],
                        Password = context.Configuration["ConnectionSecrets:PaymentContextPassword"]
                    };
                    var dbConnectionString = builderConnectionStringBuilder.ConnectionString;
                    // DbContext needs to be transient, otherwise access to db from Dataverse synchronizer and payments business logic can have conflicts
                    services.AddDbContext<PaymentContext>(options => options.UseSqlServer(dbConnectionString), ServiceLifetime.Transient, ServiceLifetime.Transient);

                    services.AddPaymentProcessors();

                    // These services are needed for the DataPush
                    services.AddSingleton<EntitiesSequenceBackgroundService>();
                    services.AddSingleton<DataPushService>();
                    services.AddSingleton<IXrmService, XrmService>();

                    var dataverseConnectionString = context.Configuration["ConnectionSecrets:DataverseConnectionString"];
                    services.AddScoped<IOrganizationService>(sp => new ServiceClient(dataverseConnectionString));
                    services.AddTransient<IPaymentContext>(p => p.GetRequiredService<PaymentContext>());
                    services.AddTransient<IPaymentRepoService, PaymentRepoService>();
                    services.AddTransient<IEntityRepository, DbEntityRepository>();
                    services.AddTransient<IDataVersionTokenRepository, DbDataVersionTokenRepository>();
                    services.AddTransient<IDataverseSynchronizer, ChangeTrackingDataverseSynchronizer>();
                    services.AddTransient<IDbEntitySynchronizer, DbEntitySynchronizer>(sp => DbEntitySynchronizer.CreateWithConfiguredContext(
                        sp.GetRequiredService<DbContextOptions<PaymentContext>>(),
                        sp.GetRequiredService<IOrganizationService>(),
                        sp.GetRequiredService<ILogger>(),
                        new FullSyncHandler(
                            sp.GetRequiredService<EntitiesSequenceBackgroundService>(),
                            sp.GetRequiredService<ILogger>()).PushEntitiesToDataverseSynchronously
                    ));
                    services.AddHostedService<RecurringDonationBackgroundService>();
                })
                .UseConsoleLifetime();
        }
    }
}