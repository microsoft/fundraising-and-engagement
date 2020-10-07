using FundraisingandEngagement.Data;
using FundraisingandEngagement.DataFactory.Workers;

namespace FundraisingandEngagement.DataFactory
{
	public class DataFactory
	{
		private PaymentContext _context;

		public DataFactory(PaymentContext context)
		{
			_context = context;
		}

		public FactoryManager GetDataFactory<T>()
		{
			var type = typeof(T);

			switch (type.Name)
			{
				case "Account":
					return new AccountWorker(_context);

				case "BankRun":
					return new BankRunWorker(_context);

				case "BankRunSchedule":
					return new BankRunScheduleWorker(_context);

				case "Configuration":
					return new ConfigurationWorker(_context);

				case "Contact":
					return new ContactWorker(_context);

				case "TransactionCurrency":
					return new TransactionCurrencyWorker(_context);

				case "Designation":
					return new DesignationWorker(_context);

				case "Event":
					return new EventWorker(_context);

				case "EventDonation":
					return new EventDonationWorker(_context);

				case "EventPackage":
					return new EventPackageWorker(_context);

				case "EventProduct":
					return new EventProductWorker(_context);

				case "EventSponsor":
					return new EventSponsorWorker(_context);

				case "EventSponsorship":
					return new EventSponsorshipWorker(_context);

				case "EventTicket":
					return new EventTicketWorker(_context);

				case "GiftAidDeclaration":
					return new GiftAidDeclarationWorker(_context);

				case "Membership":
					return new MembershipWorker(_context);

				case "MembershipCategory":
					return new MembershipCategoryWorker(_context);

				case "MembershipGroup":
					return new MembershipGroupWorker(_context);

				case "MembershipOrder":
					return new MembershipOrderWorker(_context);

				case "Payment":
					return new PaymentWorker(_context);

				case "PaymentMethod":
					return new PaymentMethodWorker(_context);

				case "PaymentProcessor":
					return new PaymentProcessorWorker(_context);

				case "Product":
					return new ProductWorker(_context);

				case "Receipt":
					return new ReceiptWorker(_context);

				case "ReceiptLog":
					return new ReceiptLogWorker(_context);

				case "ReceiptStack":
					return new ReceiptStackWorker(_context);

				case "Refund":
					return new RefundWorker(_context);

				case "Registration":
					return new RegistrationWorker(_context);

				//case "RelatedImage":
				//    return new RelatedImageWorker(_context);

				case "Response":
					return new ResponseWorker(_context);

				case "Sponsorship":
					return new SponsorshipWorker(_context);

				//case "TermsOfReference":
				//    return new TermsOfReferenceWorker(_context);

				case "Ticket":
					return new TicketWorker(_context);

				case "Transaction":
					return new TransactionWorker(_context);

				case "TributeOrMemory":
					return new TributeOrMemoryWorker(_context);

				case "PaymentSchedule":
					return new PaymentScheduleWorker(_context);

				//case "FloorWorker":
				//    return new FloorWorker(_context);

				case "PreferenceCategory":
					return new PreferenceCategoryWorker(_context);

				case "Preference":
					return new PreferenceWorker(_context);

				case "EventPreference":
					return new EventPreferenceWorker(_context);

				case "RegistrationPreference":
					return new RegistrationPreferenceWorker(_context);

				case "EventTable":
					return new EventTableWorker(_context);

				//case "Payment":
				//    return new PaymentWorker(_context);

				//case "PageOrder":
				//    return new PageOrderWorker(_context);

				case "EventDisclaimer":
					return new EventDisclaimerWorker(_context);

				case "DonorCommitment":
					return new DonorCommitmentWorker(_context);

			}

			return null;
		}

	}
}
