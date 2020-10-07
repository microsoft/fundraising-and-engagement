using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PaymentProcessors.StripeIntegration.Helpers;
using PaymentProcessors.StripeIntegration.Model;

namespace PaymentProcessors.Stripe
{
	public class StripeCustomerService : StripeService
	{
		public StripeCustomerService(string apiKey = null) : base(apiKey) { }

		public bool ExpandDefaultSource { get; set; }
		public bool ExpandDefaultCustomerBankAccount { get; set; }


		//Async
		public virtual async Task<StripeCustomer> CreateAsync(StripeCustomerCreateOptions createOptions, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
		{
			return Mapper<StripeCustomer>.MapFromJson(
				await Requestor.PostStringAsync(this.ApplyAllParameters(createOptions, Urls.Customers, false),
				SetupRequestOptions(requestOptions),
				cancellationToken)
			);
		}

		public virtual async Task<StripeCustomer> GetAsync(string customerId, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
		{
			return Mapper<StripeCustomer>.MapFromJson(
				await Requestor.GetStringAsync(this.ApplyAllParameters(null, $"{Urls.Customers}/{customerId}", false),
				SetupRequestOptions(requestOptions),
				cancellationToken)
			);
		}

		public virtual async Task<StripeCustomer> UpdateAsync(string customerId, StripeCustomerUpdateOptions updateOptions, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
		{
			return Mapper<StripeCustomer>.MapFromJson(
				await Requestor.PostStringAsync(this.ApplyAllParameters(updateOptions, $"{Urls.Customers}/{customerId}", false),
				SetupRequestOptions(requestOptions),
				cancellationToken)
			);
		}

		public virtual async Task<StripeDeleted> DeleteAsync(string customerId, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
		{
			return Mapper<StripeDeleted>.MapFromJson(
				await Requestor.DeleteAsync($"{Urls.Customers}/{customerId}",
				SetupRequestOptions(requestOptions),
				cancellationToken)
			);
		}

		public virtual async Task<IEnumerable<StripeCustomer>> ListAsync(StripeCustomerListOptions listOptions = null, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
		{
			return Mapper<StripeCustomer>.MapCollectionFromJson(
				await Requestor.GetStringAsync(this.ApplyAllParameters(listOptions, Urls.Customers, true),
				SetupRequestOptions(requestOptions),
				cancellationToken)
			);
		}
	}

	public class CustomerServiceBaseStipeRepository
	{
		private const string customerStripeUrl = "https://api.stripe.com/v1/customers";
		private readonly BaseStipeRepository stipeRepository;

		public CustomerServiceBaseStipeRepository(HttpClient httpClient)
		{
			this.stipeRepository = new BaseStipeRepository(httpClient);
		}

		public Task<StripeCustomer> GetStripeCustomerAsync(string custName, string custEmail, string strToken, CancellationToken cancellationToken)
		{
			var customer = new StripeCustomer()
			{
				Email = custEmail,
				Description = custName
			};

			return this.stipeRepository.CreateAsync(customer, customerStripeUrl, strToken, cancellationToken);
		}
	}
}
