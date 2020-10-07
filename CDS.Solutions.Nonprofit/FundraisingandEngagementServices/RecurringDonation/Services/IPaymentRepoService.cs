using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.RecurringDonations.Services
{
	public interface IPaymentRepoService
	{
		Task<IReadOnlyList<SinglePaymentVariables>> GetFailedPaymentsAsync(CancellationToken token = default);

		Task<IReadOnlyList<SinglePaymentVariables>> GetScheduledPaymentsAsync(CancellationToken token = default);

		Task SaveChangesAsync(CancellationToken token, Response response);
	}
}