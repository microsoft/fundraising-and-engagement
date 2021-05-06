using System;
using System.Diagnostics;
using System.Threading;
using FundraisingandEngagement.Services.DataPush;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement.Services
{
    public class FullSyncHandler
    {
        private bool _dataPushDone = false;
        private readonly EntitiesSequenceBackgroundService _dataPushService;
        private readonly ILogger _logger;

        public FullSyncHandler(EntitiesSequenceBackgroundService dataPushService, ILogger logger)
        {
            _dataPushService = dataPushService;
            _logger = logger;
        }

        // Ensure that DataPush succeeds before running full sync.
        public void PushEntitiesToDataverseSynchronously(string entityToSync)
        {
            if (!_dataPushDone)
            {
                try
                {
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    _logger.LogInformation("Starting datapush before full data synchronization.");
                    var cancelToken = new CancellationToken();
                    _dataPushService.ExecuteAsync(cancelToken, failOnException: true).GetAwaiter().GetResult();
                    _dataPushDone = true;
                    _logger.LogInformation("Completed datapush in {0}.", stopWatch.Elapsed);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Critical error: datapush failed. The data synchronization cannot continue if DataPush is failing.", ex);
                    throw;
                }
            }
        }
    }
}