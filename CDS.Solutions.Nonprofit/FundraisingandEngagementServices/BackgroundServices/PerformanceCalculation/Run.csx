#r "../bin/FundraisingandEngagement.BackgroundServices.dll"

using System.Threading;
using System.Threading.Tasks;
using FundraisingandEngagement.BackgroundServices;

public static async Task Run(TimerInfo performanceCalculationTimer, ILogger log, CancellationToken token)
{
	await Program.RunPerfomanceCalculationAsync(null, log, token);
}