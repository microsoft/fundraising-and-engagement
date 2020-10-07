using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FundraisingandEngagement.Models.Entities;
using System.Linq.Expressions;

namespace FundraisingandEngagement.Data
{
	public partial class PaymentContext : DbContext, IPaymentContext
	{
		IQueryable<Account> IPaymentContext.Account => Set<Account>();

		IQueryable<Configuration> IPaymentContext.Configuration => Set<Configuration>();

		IQueryable<Contact> IPaymentContext.Contact => Set<Contact>();

		IQueryable<Designation> IPaymentContext.Designation => Set<Designation>();

		IQueryable<Event> IPaymentContext.Event => Set<Event>();

		IQueryable<EventDisclaimer> IPaymentContext.EventDisclaimer => Set<EventDisclaimer>();

		IQueryable<EventDonation> IPaymentContext.EventDonation => Set<EventDonation>();

		IQueryable<EventPackage> IPaymentContext.EventPackage => Set<EventPackage>();

		IQueryable<EventPreference> IPaymentContext.EventPreference => Set<EventPreference>();

		IQueryable<EventProduct> IPaymentContext.EventProduct => Set<EventProduct>();

		IQueryable<EventSponsor> IPaymentContext.EventSponsor => Set<EventSponsor>();

        IQueryable<EventSponsorship> IPaymentContext.EventSponsorship => Set<EventSponsorship>();

		IQueryable<EventTable> IPaymentContext.EventTable => Set<EventTable>();

		IQueryable<EventTicket> IPaymentContext.EventTicket => Set<EventTicket>();

		IQueryable<GiftAidDeclaration> IPaymentContext.GiftAidDeclaration => Set<GiftAidDeclaration>();

		IQueryable<Membership> IPaymentContext.Membership => Set<Membership>();

		IQueryable<MembershipCategory> IPaymentContext.MembershipCategory => Set<MembershipCategory>();

		IQueryable<MembershipGroup> IPaymentContext.MembershipGroup => Set<MembershipGroup>();

		IQueryable<MembershipOrder> IPaymentContext.MembershipOrder => Set<MembershipOrder>();

		IQueryable<PageOrder> IPaymentContext.PageOrder => Set<PageOrder>();

		IQueryable<Payment> IPaymentContext.Payment => Set<Payment>();

		IQueryable<PaymentMethod> IPaymentContext.PaymentMethod => Set<PaymentMethod>();

		IQueryable<PaymentProcessor> IPaymentContext.PaymentProcessor => Set<PaymentProcessor>();

		IQueryable<PaymentSchedule> IPaymentContext.PaymentSchedule => Set<PaymentSchedule>();

		IQueryable<Preference> IPaymentContext.Preference => Set<Preference>();

		IQueryable<PreferenceCategory> IPaymentContext.PreferenceCategory => Set<PreferenceCategory>();

		IQueryable<Product> IPaymentContext.Product => Set<Product>();

		IQueryable<Receipt> IPaymentContext.Receipt => Set<Receipt>();

		IQueryable<ReceiptLog> IPaymentContext.ReceiptLog => Set<ReceiptLog>();

		IQueryable<ReceiptStack> IPaymentContext.ReceiptStack => Set<ReceiptStack>();

		IQueryable<Refund> IPaymentContext.Refund => Set<Refund>();

		IQueryable<Registration> IPaymentContext.Registration => Set<Registration>();

		IQueryable<RegistrationPreference> IPaymentContext.RegistrationPreference => Set<RegistrationPreference>();

		IQueryable<RelatedImage> IPaymentContext.RelatedImage => Set<RelatedImage>();

		IQueryable<Response> IPaymentContext.Response => Set<Response>();

		IQueryable<Sponsorship> IPaymentContext.Sponsorship => Set<Sponsorship>();

		IQueryable<SyncLog> IPaymentContext.SyncException => Set<SyncLog>();

		IQueryable<TermsOfReference> IPaymentContext.TermsOfReference => Set<TermsOfReference>();

		IQueryable<Ticket> IPaymentContext.Ticket => Set<Ticket>();

		IQueryable<Transaction> IPaymentContext.Transaction => Set<Transaction>();

		IQueryable<TransactionCurrency> IPaymentContext.TransactionCurrency => Set<TransactionCurrency>();

		IQueryable<TributeOrMemory> IPaymentContext.TributeOrMemory => Set<TributeOrMemory>();


		public async Task<T> FindAsync<T>(object id) where T : class
		{
			return await Set<T>().FindAsync(id);
		}

		void IPaymentContext.Add<T>(T entity) => Add(entity);

		void IPaymentContext.EntryAdd<T>(T entity) => Entry(entity).State = EntityState.Added;

		void IPaymentContext.EntryModify<T>(T entity) => Entry(entity).State = EntityState.Modified;

		void IPaymentContext.EntryPropertyModify<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression) where TEntity : class => Entry(entity).Property(propertyExpression).IsModified = true;

		IQueryable<T> IPaymentContext.Set<T>() where T : class => Set<T>();

		public Guid GetPK<T>(T entity) where T : class
		{
			var entry = Entry(entity);

			var keyParts = entry
				.Metadata
				.FindPrimaryKey()
				.Properties
				.Select(p => entry.Property(p.Name).CurrentValue)
				.ToArray();

			if (keyParts.Length == 1 && keyParts[0] is Guid id)
				return id;

			return Guid.Empty;
		}
	}
}
