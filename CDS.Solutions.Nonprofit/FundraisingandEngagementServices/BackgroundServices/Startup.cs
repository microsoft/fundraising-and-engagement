using System;
using System.IO;
using Data.DataSynchronization;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Data.DataSynchronization;
using FundraisingandEngagement.DataverseSynchronization;
using FundraisingandEngagement.Models.Options;
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
                    services.Configure<CrmOptions>(options =>
                    {
                        context.Configuration.GetSection("Crm").Bind(options);
                        options.CrmApplicationKey = context.Configuration["ConnectionSecrets:DataverseClientSecret"];
                    });

                    var connectionStringBuilder = new SqlConnectionStringBuilder(context.Configuration.GetConnectionString("PaymentContext"))
                    {
                        UserID = context.Configuration["ConnectionSecrets:PaymentContextUserID"],
                        Password = context.Configuration["ConnectionSecrets:PaymentContextPassword"]
                    };

                    services.AddDbContext<PaymentContext>(options => options.UseSqlServer(connectionStringBuilder.ConnectionString), ServiceLifetime.Transient, ServiceLifetime.Transient);
                    services.AddSingleton<ILogger>(provider => log ?? provider.GetRequiredService<ILoggerFactory>().CreateLogger("BackgroundServices"));
                    services.AddHttpClient();

                    services.AddSingleton<IPaymentContext>(provider => provider.GetRequiredService<PaymentContext>());
                    services.AddSingleton<IXrmService, XrmService>();
                    services.AddSingleton<EntitiesSequenceBackgroundService>();
                    services.AddSingleton<DataPushService>();
                    services.AddSingleton<EventReceiptingOperations>();
                    services.AddSingleton<BankRunOperations>();
                    services.AddSingleton<IYearlyGivingCalculator, YearlyGivingCalculator>();
                    services.AddSingleton<YearlyGivingFromEntity>();
                    services.AddSingleton<YearlyGivingFull>(sp =>
                        new YearlyGivingFull(
                            () => sp.GetRequiredService<PaymentContext>(),
                            sp.GetRequiredService<IYearlyGivingCalculator>(),
                            sp.GetRequiredService<ILoggerFactory>())
                    );
                    services.AddSingleton<PerformanceCalculation>();
                    services.AddSingleton<EventReceipting>();

                    services.AddScoped<IOrganizationService>(sp => new ServiceClient(context.Configuration["ConnectionSecrets:DataverseConnectionString"]));
                    services.AddTransient<IPaymentContext>(p => p.GetRequiredService<PaymentContext>());
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
                });
        }
    }
}