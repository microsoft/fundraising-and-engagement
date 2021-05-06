using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.DataFactory.Workers;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory
{
    public class DbDataFactory : IDataFactory
    {
        private PaymentContext _context;

        public DbDataFactory(PaymentContext context)
        {
            _context = context;
        }

        public IFactoryFloor<T> GetDataFactory<T>() where T : PaymentEntity
        {
            var type = typeof(T);
            switch (type.Name)
            {
                case "Account":
                    return (IFactoryFloor<T>)new AccountWorker(this._context);

                case "BankRun":
                    return (IFactoryFloor<T>)new BankRunWorker(this._context);

                case "BankRunSchedule":
                    return (IFactoryFloor<T>)new BankRunScheduleWorker(_context);

                case "Configuration":
                    return (IFactoryFloor<T>)new ConfigurationWorker(_context);

                case "Contact":
                    return (IFactoryFloor<T>)new ContactWorker(_context);

                case "TransactionCurrency":
                    return (IFactoryFloor<T>)new TransactionCurrencyWorker(_context);

                case "Designation":
                    return (IFactoryFloor<T>)new DesignationWorker(_context);

                case "Event":
                    return (IFactoryFloor<T>)new EventWorker(_context);

                case "EventDonation":
                    return (IFactoryFloor<T>)new EventDonationWorker(_context);

                case "EventPackage":
                    return (IFactoryFloor<T>)new EventPackageWorker(_context);

                case "EventProduct":
                    return (IFactoryFloor<T>)new EventProductWorker(_context);

                case "EventSponsor":
                    return (IFactoryFloor<T>)new EventSponsorWorker(_context);

                case "EventSponsorship":
                    return (IFactoryFloor<T>)new EventSponsorshipWorker(_context);

                case "EventTicket":
                    return (IFactoryFloor<T>)new EventTicketWorker(_context);

                case "GiftAidDeclaration":
                    return (IFactoryFloor<T>)new GiftAidDeclarationWorker(_context);

                case "Membership":
                    return (IFactoryFloor<T>)new MembershipWorker(_context);

                case "MembershipCategory":
                    return (IFactoryFloor<T>)new MembershipCategoryWorker(_context);

                case "MembershipGroup":
                    return (IFactoryFloor<T>)new MembershipGroupWorker(_context);

                case "MembershipOrder":
                    return (IFactoryFloor<T>)new MembershipOrderWorker(_context);

                case "Payment":
                    return (IFactoryFloor<T>)new PaymentWorker(_context);

                case "PaymentMethod":
                    return (IFactoryFloor<T>)new PaymentMethodWorker(_context);

                case "PaymentProcessor":
                    return (IFactoryFloor<T>)new PaymentProcessorWorker(_context);

                case "Product":
                    return (IFactoryFloor<T>)new ProductWorker(_context);

                case "Receipt":
                    return (IFactoryFloor<T>)new ReceiptWorker(_context);

                case "ReceiptLog":
                    return (IFactoryFloor<T>)new ReceiptLogWorker(_context);

                case "ReceiptStack":
                    return (IFactoryFloor<T>)new ReceiptStackWorker(_context);

                case "Refund":
                    return (IFactoryFloor<T>)new RefundWorker(_context);

                case "Registration":
                    return (IFactoryFloor<T>)new RegistrationWorker(_context);

                //case "RelatedImage":
                //    return (IFactoryFloor<T>) new RelatedImageWorker(_context);

                case "Response":
                    return (IFactoryFloor<T>)new ResponseWorker(_context);

                case "Sponsorship":
                    return (IFactoryFloor<T>)new SponsorshipWorker(_context);

                case "Ticket":
                    return (IFactoryFloor<T>)new TicketWorker(_context);

                case "Transaction":
                    return (IFactoryFloor<T>)new TransactionWorker(_context);

                case "TributeOrMemory":
                    return (IFactoryFloor<T>)new TributeOrMemoryWorker(_context);

                case "PaymentSchedule":
                    return (IFactoryFloor<T>)new PaymentScheduleWorker(_context);

                //case "FloorWorker":
                //    return (IFactoryFloor<T>) new FloorWorker(_context);

                case "PreferenceCategory":
                    return (IFactoryFloor<T>)new PreferenceCategoryWorker(_context);

                case "Preference":
                    return (IFactoryFloor<T>)new PreferenceWorker(_context);

                case "EventPreference":
                    return (IFactoryFloor<T>)new EventPreferenceWorker(_context);

                case "RegistrationPreference":
                    return (IFactoryFloor<T>)new RegistrationPreferenceWorker(_context);

                case "EventTable":
                    return (IFactoryFloor<T>)new EventTableWorker(_context);

                //case "Payment":
                //    return (IFactoryFloor<T>) new PaymentWorker(_context);

                //case "PageOrder":
                //    return (IFactoryFloor<T>) new PageOrderWorker(_context);

                case "EventDisclaimer":
                    return (IFactoryFloor<T>)new EventDisclaimerWorker(_context);

                case "DonorCommitment":
                    return (IFactoryFloor<T>)new DonorCommitmentWorker(_context);
            }

            return null;
        }
    }
}