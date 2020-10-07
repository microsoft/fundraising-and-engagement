#r "../bin/FundraisingandEngagement.BackgroundServices.dll"

using System.Threading;
using System.Threading.Tasks;
using FundraisingandEngagement.BackgroundServices;

public static async Task Run(TimerInfo yearlyCalculationTimer, ILogger log, CancellationToken token)
{
	await Program.RunYearlyGivingFullRecalculationAsync(null, log, token);
}