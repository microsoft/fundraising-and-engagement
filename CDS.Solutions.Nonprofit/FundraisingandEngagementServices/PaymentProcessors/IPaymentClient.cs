using System.Threading;
using System.Threading.Tasks;

namespace PaymentProcessors
{
	internal interface IPaymentClient<in TInput, TResult>
	{
		Task<TResult> MakePaymentAsync(TInput input, CancellationToken cancellationToken = default);
	}
}
