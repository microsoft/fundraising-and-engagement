using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FundraisingandEngagement.BackgroundServices.DataPush;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement.BackgroundServices
{
	public static class Program
	{
		public static Task Main(string[] args) => RanCommand(args);

		public static async Task RunBankAsync(HttpRequestMessage request, string action, Guid id, ILogger log, CancellationToken token)
		{
			await RanCommand(new[] { "bankRun", "--action", action, "--id", $"{id}" }, log, token);
		}

		public static async Task RunDataPushAsync(HttpRequestMessage request, ILogger log, CancellationToken token)
		{
			await RanCommand(new[] { "dataPush" }, log, token);
		}

		public static async Task RunEventReceiptingAsync(HttpRequestMessage request, string entityName, Guid entityId, ILogger log, CancellationToken token)
		{
			await RanCommand(new[] { "eventReceipting", "--entityName", entityName, "--entityId", $"{entityId}" }, log, token);
		}

		public static async Task RunYearlyGivingCalculationAsync(HttpRequestMessage request, string entityName, Guid id, ILogger log, CancellationToken token)
		{
			await RanCommand(new[] { "yearlyGiving", "--entityName", entityName, "--entityId", $"{id}" }, log, token);
		}

		public static async Task RunYearlyGivingFullRecalculationAsync(HttpRequestMessage request, ILogger log, CancellationToken token)
		{
			await RanCommand(new[] { "yearlyGivingFull" }, log, token);
		}

		public static async Task RunPerfomanceCalculationAsync(HttpRequestMessage request, ILogger log, CancellationToken cancellationToken)
		{
			await RanCommand(new[] { "performanceCalculation" }, log, cancellationToken);
		}


		private static async Task RanCommand(string[] args, ILogger? log = null, CancellationToken token = default)
		{
			if (args == null || args.Length == 0)
			{
				log?.LogWarning("Missing command arguments");
				return;
			}

			log?.LogInformation($"Starting {String.Join(" ", args)}");

			var rootCommand = new RootCommand();
			rootCommand.AddCommand(BankRunCommand());
			rootCommand.AddCommand(DataPushCommand());
			rootCommand.AddCommand(EventReceiptingCommand());
			rootCommand.AddCommand(YearlyGivingCalculationCommand());
			rootCommand.AddCommand(YearlyGivingFullRecalculationCommand());
			rootCommand.AddCommand(PerfomanceCalculationCommand());
			rootCommand.Handler = CommandHandler.Create((IHelpBuilder help) => help.Write(rootCommand));

			var parser = new CommandLineBuilder(rootCommand);

			await parser
				.UseHelp()
				.UseParseErrorReporting()
				.UseHost(Host.CreateDefaultBuilder, builder => builder.ConfigureHostBuilder(log))
				.Build()
				.InvokeAsync(args);

			log?.LogInformation($"Existing {args[0]}");
		}


		private static Command BankRunCommand()
		{
			var command = new Command("bankRun", "Invokes one of Bank Run actions based on action parameter");
			command.AddOption(new Option<string>("--action", "One of List, File or GenerateTransactions"));
			command.AddOption(new Option<Guid>("--id", "GUID of BankRun entity (aka Primary Key)"));
			command.Handler = CommandHandler.Create<IHost, string, Guid, CancellationToken>((host, action, id, token) => host.Services.GetRequiredService<Functions>().BankRunAppSelector(action, id, null, String.Empty));
			return command;
		}

		private static Command DataPushCommand()
		{
			var command = new Command("dataPush", "Pushes data from Azure to Dynamics (via Dynamics RESTful API)");
			command.Handler = CommandHandler.Create<IHost, CancellationToken>((host, token) => host.Services.GetRequiredService<EntitiesSequenceBackgroundService>().ExecuteAsync(token));
			return command;
		}

		private static Command EventReceiptingCommand()
		{
			var command = new Command("eventReceipting", "Event Receipting");
			command.AddOption(new Option<string>("--entityName", "Name of Entity, as known in Dynamics"));
			command.AddOption(new Option<Guid>("--entityId", "GUID of Entity (aka Primary Key)"));
			command.Handler = CommandHandler.Create<IHost, string, Guid, CancellationToken>((host, entityName, entityId, token) => host.Services.GetRequiredService<Functions>().BankRunAppSelector("EventReceipting", null, entityId, entityName));
			return command;
		}

		private static Command YearlyGivingCalculationCommand()
		{
			var command = new Command("yearlyGiving", "Yearly Giving Calculation");
			command.AddOption(new Option<string>("--entityName", "Name of Entity, as known in Dynamics"));
			command.AddOption(new Option<Guid>("--entityId", "GUID of Entity (aka Primary Key)"));
			command.Handler = CommandHandler.Create<IHost, string, Guid, CancellationToken>((host, entityName, entityId, token) => host.Services.GetRequiredService<YearlyGivingFromEntity>().CalculateFromPaymentEntity(entityId, entityName));
			return command;
		}

		private static Command YearlyGivingFullRecalculationCommand()
		{
			var command = new Command("yearlyGivingFull", "Yearly Giving Full Recalculation");
			command.Handler = CommandHandler.Create<IHost, CancellationToken>((host, token) => host.Services.GetRequiredService<YearlyGivingFull>().FullRecalculation());
			return command;
		}

		private static Command PerfomanceCalculationCommand()
		{
			var command = new Command("performanceCalculation", "Performance calculation for Campaign / Appeal / Package");
			command.Handler = CommandHandler.Create<IHost, CancellationToken>((host, token) => host.Services.GetRequiredService<PerformanceCalculation>().PerformanceMetricsStart(token));
			return command;
		}
	}
}