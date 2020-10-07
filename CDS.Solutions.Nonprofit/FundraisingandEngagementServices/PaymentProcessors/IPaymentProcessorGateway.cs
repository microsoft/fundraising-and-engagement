using System.Threading;
using System.Threading.Tasks;
using PaymentProcessors.Models;

namespace PaymentProcessors
{
	public interface IPaymentProcessorGateway
	{
		/// <remarks>
		/// This method will throw exceptions.
		/// It is responsibility of the caller to handle exceptions
		/// </remarks>
		Task<PaymentOutput> MakePaymentAsync(PaymentInput input, CancellationToken token = default);

		/// <remarks>
		/// This method will throw exceptions.
		/// It is responsibility of the caller to handle exceptions
		/// </remarks>
		Task<RecurringPaymentOutput> MakePreAuthorizedPaymentAsync(RecurringPaymentInput request, CancellationToken token = default);
	}
}