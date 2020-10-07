using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;

namespace FundraisingandEngagement.BackgroundServices.DataPush
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

		public async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				await PushEntitiesAsync(stoppingToken);
			}
			catch (Exception e)
			{
				this.logger.LogCritical(e, "Critical error");
			}
		}

		private async Task PushEntitiesAsync(CancellationToken stoppingToken)
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

			await this.dataPush.PushAsync<BankRun>(stoppingToken);
			await this.dataPush.PushAsync<BankRunSchedule>(stoppingToken);
			await this.dataPush.PushAsync<Note>(stoppingToken);
			await this.dataPush.PushAsync<PaymentSchedule>(stoppingToken);
			await this.dataPush.PushAsync<EventPackage>(stoppingToken);
			await this.dataPush.PushAsync<EventTicket>(stoppingToken);
			await this.dataPush.PushAsync<Ticket>(stoppingToken);
			await this.dataPush.PushAsync<EventProduct>(stoppingToken);
			await this.dataPush.PushAsync<Product>(stoppingToken);
			await this.dataPush.PushAsync<EventSponsorship>(stoppingToken);
			await this.dataPush.PushAsync<Sponsorship>(stoppingToken);
			await this.dataPush.PushAsync<Registration>(stoppingToken);
			await this.dataPush.PushAsync<Transaction>(stoppingToken, transaction => transaction.StatusCode != StatusCode.Active);
			await this.dataPush.PushAsync<Response>(stoppingToken);
			await this.dataPush.PushAsync<Contact>(stoppingToken);
			await this.dataPush.PushAsync<Account>(stoppingToken);
		}
	}
}
