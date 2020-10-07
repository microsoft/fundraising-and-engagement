using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.Data
{
	public interface IPaymentContext
	{
		IQueryable<Account> Account { get; }
		IQueryable<Configuration> Configuration { get; }
		IQueryable<Contact> Contact { get; }
		IQueryable<Designation> Designation { get; }
		IQueryable<Event> Event { get; }
		IQueryable<EventDisclaimer> EventDisclaimer { get; }
		IQueryable<EventDonation> EventDonation { get; }
		IQueryable<EventPackage> EventPackage { get; }
		IQueryable<EventPreference> EventPreference { get; }
		IQueryable<EventProduct> EventProduct { get; }
		IQueryable<EventSponsor> EventSponsor { get; }
        IQueryable<EventSponsorship> EventSponsorship { get; }
		IQueryable<EventTable> EventTable { get; }
		IQueryable<EventTicket> EventTicket { get; }
		IQueryable<GiftAidDeclaration> GiftAidDeclaration { get; }
		IQueryable<Membership> Membership { get; }
		IQueryable<MembershipCategory> MembershipCategory { get; }
		IQueryable<MembershipGroup> MembershipGroup { get; }
		IQueryable<MembershipOrder> MembershipOrder { get; }
		IQueryable<PageOrder> PageOrder { get; }
		IQueryable<Payment> Payment { get; }
		IQueryable<PaymentMethod> PaymentMethod { get; }
		IQueryable<PaymentProcessor> PaymentProcessor { get; }
		IQueryable<PaymentSchedule> PaymentSchedule { get; }
		IQueryable<Preference> Preference { get; }
		IQueryable<PreferenceCategory> PreferenceCategory { get; }

		IQueryable<Product> Product { get; }

		IQueryable<Receipt> Receipt { get; }


		IQueryable<ReceiptLog> ReceiptLog { get; }

		IQueryable<ReceiptStack> ReceiptStack { get; }

		IQueryable<Refund> Refund { get; }
		IQueryable<Registration> Registration { get; }
		IQueryable<RegistrationPreference> RegistrationPreference { get; }
		IQueryable<RelatedImage> RelatedImage { get; }
		IQueryable<Response> Response { get; }
		IQueryable<Sponsorship> Sponsorship { get; }
		IQueryable<SyncLog> SyncException { get; }
		IQueryable<TermsOfReference> TermsOfReference { get; }
		IQueryable<Ticket> Ticket { get; }
		IQueryable<Transaction> Transaction { get; }
		IQueryable<TransactionCurrency> TransactionCurrency { get; }
		IQueryable<TributeOrMemory> TributeOrMemory { get; }


		Task<T> FindAsync<T>(object id) where T : class;

		void Add<T>(T entity);

		void EntryAdd<T>(T entity);

		void EntryModify<T>(T entity);

		void EntryPropertyModify<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression) where TEntity : class;

		IQueryable<T> Set<T>() where T : class;

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

		Guid GetPK<T>(T entity) where T : class;
	}
}