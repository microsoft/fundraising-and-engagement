using System;
using System.Threading;
using System.Threading.Tasks;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement.Services.DataPush
{
    public class EntitiesSequenceBackgroundService
    {
        private readonly DataPushService dataPush;
        private readonly ILogger logger;

        public EntitiesSequenceBackgroundService(DataPushService dataPush, ILogger logger)
        {
            this.logger = logger;
            this.dataPush = dataPush;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken, bool failOnException = false)
        {
            try
            {
                await PushEntitiesAsync(stoppingToken, failOnException);
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e, "Critical error");
                if (failOnException)
                {
                    throw;
                }
            }
        }

        private async Task PushEntitiesAsync(CancellationToken stoppingToken, bool failOnException)
        {
            // If you are adding an enitity to the list below, ensure that the entity:
            //
            // 1) Has mapping attributes with field names matching exactly as they are in Dynamics
            //
            // 2) Is placed in right order, eg:
            //    a) Transactions have to be pushed to Dynamics before Responses can be send
            //    b) PaymentMethod have to be pushed to Dynamics before Transactions can be send
            //    ... and so on, depending on the relationship between entities
            //
            // ... otherwise data push WILL fail for the entity you are trying to add
            //
            // **************************************************************************
            // ** After changes, test by running the app, it should have 'clean' exit  **
            // ** (no red errors) as there is validation logic in Dynamics and Plugins **
            // ** that can only seen by running the app                                **
            // **************************************************************************
            // 
            // To run DataPush, in launchSettings.json, set 'commandLineArgs' to 'dataPush'
            //

            await this.dataPush.PushAsync<BankRun>(stoppingToken, failOnException);
            await this.dataPush.PushAsync<BankRunSchedule>(stoppingToken, failOnException);
            await this.dataPush.PushAsync<Note>(stoppingToken, failOnException);
            await this.dataPush.PushAsync<PaymentSchedule>(stoppingToken, failOnException);
            await this.dataPush.PushAsync<EventPackage>(stoppingToken, failOnException);
            await this.dataPush.PushAsync<EventTicket>(stoppingToken, failOnException); // jmichelf: I'm not removing this just to be sure, but I don't see any changes to this entity from Azure side
            await this.dataPush.PushAsync<Ticket>(stoppingToken, failOnException);
            await this.dataPush.PushAsync<EventProduct>(stoppingToken, failOnException); // jmichelf: I'm not removing this just to be sure, but I don't see any changes to this entity from Azure side
            await this.dataPush.PushAsync<Product>(stoppingToken, failOnException);
            await this.dataPush.PushAsync<EventSponsorship>(stoppingToken, failOnException); // jmichelf: I'm not removing this just to be sure, but I don't see any changes to this entity from Azure side
            await this.dataPush.PushAsync<Sponsorship>(stoppingToken, failOnException);
            await this.dataPush.PushAsync<Registration>(stoppingToken, failOnException); // jmichelf: I'm not removing this just to be sure, but I don't see any changes to this entity from Azure side
            await this.dataPush.PushAsync<Transaction>(stoppingToken, failOnException, transaction => transaction.StatusCode != StatusCode.Active);
            await this.dataPush.PushAsync<Response>(stoppingToken, failOnException);
            await this.dataPush.PushAsync<Contact>(stoppingToken, failOnException);
            await this.dataPush.PushAsync<Account>(stoppingToken, failOnException);
        }
    }
}