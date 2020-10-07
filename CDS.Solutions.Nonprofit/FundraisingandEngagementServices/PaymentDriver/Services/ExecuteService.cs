using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace PaymentDriver.Services
{
	internal class ExecuteService : BackgroundService
	{
		private readonly ICreatePayment _createPayment;

		public ExecuteService(ICreatePayment createPayment)
		{
			//load data
			this._createPayment = createPayment;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await this._createPayment.Run();
		}
	}
}
