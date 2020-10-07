using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;


namespace API
{
    public class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig = config.Build();

                    if (!String.IsNullOrEmpty(builtConfig["KeyVaultName"]))
                    {
                        config.AddAzureKeyVault($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/");
                    }
                })
                .UseStartup<Startup>();
        }
    }
}
