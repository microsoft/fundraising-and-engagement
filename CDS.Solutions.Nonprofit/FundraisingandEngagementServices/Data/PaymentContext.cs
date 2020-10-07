using Microsoft.EntityFrameworkCore;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.Data
{
	public partial class PaymentContext : DbContext
    {
        public PaymentContext()
        {
        }

        public PaymentContext(DbContextOptions<PaymentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<BankRun> BankRun { get; set; }
        public virtual DbSet<BankRunSchedule> BankRunSchedule { get; set; }
        public virtual DbSet<Configuration> Configuration { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Designation> Designation { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventDisclaimer> EventDisclaimer { get; set; }
        public virtual DbSet<EventDonation> EventDonation { get; set; }
        public virtual DbSet<EventPackage> EventPackage { get; set; }
        public virtual DbSet<EventProduct> EventProduct { get; set; }
        public virtual DbSet<EventSponsorship> EventSponsorship { get; set; }
        public virtual DbSet<EventSponsor> EventSponsor { get; set; }
        public virtual DbSet<EventTicket> EventTicket { get; set; }
        public virtual DbSet<GiftAidDeclaration> GiftAidDeclaration { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }
        public virtual DbSet<MembershipCategory> MembershipCategory { get; set; }
        public virtual DbSet<MembershipGroup> MembershipGroup { get; set; }
        public virtual DbSet<MembershipOrder> MembershipOrder { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }
        public virtual DbSet<PaymentProcessor> PaymentProcessor { get; set; }
        public virtual DbSet<PaymentSchedule> PaymentSchedule { get; set; }
        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<Receipt> Receipt { get; set; }

        //public virtual DbSet<ReceiptEmailLog> ReceiptEmailLogs { get; set; }

        public virtual DbSet<ReceiptLog> ReceiptLog { get; set; }

        public virtual DbSet<ReceiptStack> ReceiptStack { get; set; }

        public virtual DbSet<Refund> Refund { get; set; }
        public virtual DbSet<Registration> Registration { get; set; }
        //public virtual DbSet<RelatedImage> RelatedImage { get; set; }
        public virtual DbSet<Response> Response { get; set; }
        public virtual DbSet<Sponsorship> Sponsorship { get; set; }
        public virtual DbSet<SyncLog> SyncLogs { get; set; }
        //public virtual DbSet<TermsOfReference> TermsOfReference { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TributeOrMemory> TributeOrMemory { get; set; }
        public virtual DbSet<TransactionCurrency> TransactionCurrency { get; set; }
        public virtual DbSet<PreferenceCategory> PreferenceCategory { get; set; }
        public virtual DbSet<Preference> Preference { get; set; }
        public virtual DbSet<EventPreference> EventPreference { get; set; }
        public virtual DbSet<RegistrationPreference> RegistrationPreference { get; set; }
        public virtual DbSet<EventTable> EventTable { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        //public virtual DbSet<PageOrder> PageOrder { get; set; }
        public virtual DbSet<Note> Note { get; set; }
		public virtual DbSet<DonorCommitment> DonorCommitment { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasDefaultValueSql("(newid())");


                entity.Property(e => e.Address1_City).HasMaxLength(100);

                entity.Property(e => e.Address1_Country).HasMaxLength(100);

                entity.Property(e => e.Address1_County).HasMaxLength(100);

                entity.Property(e => e.Address1_Line1).HasMaxLength(100);

                entity.Property(e => e.Address1_Line2).HasMaxLength(100);

                entity.Property(e => e.Address1_Line3).HasMaxLength(100);

                entity.Property(e => e.Address1_Name).HasMaxLength(150);

                entity.Property(e => e.Address1_PostalCode).HasMaxLength(100);

                entity.Property(e => e.Address1_PostOfficeBox).HasMaxLength(100);

                entity.Property(e => e.Address1_PostOfficeBox).HasMaxLength(100);


                entity.Property(e => e.Address2_City).HasMaxLength(100);

                entity.Property(e => e.Address2_Country).HasMaxLength(100);

                entity.Property(e => e.Address2_County).HasMaxLength(100);

                entity.Property(e => e.Address2_Line1).HasMaxLength(100);

                entity.Property(e => e.Address2_Line2).HasMaxLength(100);

                entity.Property(e => e.Address2_Line3).HasMaxLength(100);

                entity.Property(e => e.Address2_Name).HasMaxLength(150);

                entity.Property(e => e.Address2_PostalCode).HasMaxLength(100);

                entity.Property(e => e.Address2_PostOfficeBox).HasMaxLength(100);

                entity.Property(e => e.Address2_PostOfficeBox).HasMaxLength(100);


                entity.Property(e => e.EmailAddress1).HasMaxLength(100);

                entity.Property(e => e.EmailAddress2).HasMaxLength(100);

                entity.Property(e => e.EmailAddress3).HasMaxLength(100);

                entity.Property(e => e.msnfp_LastEventPackageDate).HasColumnType("datetime2");

                entity.Property(e => e.msnfp_LastTransactionDate).HasColumnType("datetime2");

                entity.Property(e => e.msnfp_Sum_LifetimeTransactions).HasColumnType("money");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Telephone1).HasMaxLength(100);

                entity.Property(e => e.Telephone2).HasMaxLength(100);

                entity.Property(e => e.Telephone3).HasMaxLength(100);

                entity.Property(e => e.WebSiteURL).HasMaxLength(250);


                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.Property(e => e.msnfp_year0_giving).HasColumnType("money");
                entity.Property(e => e.msnfp_year1_giving).HasColumnType("money");
                entity.Property(e => e.msnfp_year2_giving).HasColumnType("money");
                entity.Property(e => e.msnfp_year3_giving).HasColumnType("money");
                entity.Property(e => e.msnfp_year4_giving).HasColumnType("money");
                entity.Property(e => e.msnfp_lifetimegivingsum).HasColumnType("money");

				entity.HasOne(d => d.msnfp_LastEventPackage)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.msnfp_LastEventPackageId)
                    .HasConstraintName("FK__Account__LastEventPackageId__0A688BB2");

                entity.HasOne(d => d.msnfp_LastTransaction)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.msnfp_LastTransactionId)
                    .HasConstraintName("FK__Account__LastTransactionId__0A688BB3");

                entity.HasOne(d => d.msnfp_PrimaryMembership)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.msnfp_PrimaryMembershipId)
                    .HasConstraintName("FK__Account__PrimaryMembershipId__0A688BB4");

                entity.HasOne(d => d.ParentAccount)
                    .WithMany(p => p.ChildAccount)
                    .HasForeignKey(d => d.ParentAccountId)
                    .HasConstraintName("FK__Account__ParentAccountId__0A688BB5");
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.Property(e => e.ConfigurationId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AddressAuth1).HasMaxLength(100);

                entity.Property(e => e.AddressAuth2).HasMaxLength(100);

                entity.Property(e => e.AzureWebApiUrl).HasMaxLength(100);

                entity.Property(e => e.AzureWebApp).HasMaxLength(100);

                entity.Property(e => e.CharityTitle).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.HasOne(d => d.PaymentProcessor)
                    .WithMany(p => p.Configuration)
                    .HasForeignKey(d => d.PaymentProcessorId)
                    .HasConstraintName("FK__Configura__Payme__5CA1C101");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.ContactId).HasDefaultValueSql("(newid())");


                entity.Property(e => e.Address1_City).HasMaxLength(100);

                entity.Property(e => e.Address1_Country).HasMaxLength(100);

                entity.Property(e => e.Address1_County).HasMaxLength(100);

                entity.Property(e => e.Address1_Line1).HasMaxLength(100);

                entity.Property(e => e.Address1_Line2).HasMaxLength(100);

                entity.Property(e => e.Address1_Line3).HasMaxLength(100);

                entity.Property(e => e.Address1_Name).HasMaxLength(150);

                entity.Property(e => e.Address1_PostalCode).HasMaxLength(100);

                entity.Property(e => e.Address1_PostOfficeBox).HasMaxLength(100);

                entity.Property(e => e.Address1_PostOfficeBox).HasMaxLength(100);


                entity.Property(e => e.Address2_City).HasMaxLength(100);

                entity.Property(e => e.Address2_Country).HasMaxLength(100);

                entity.Property(e => e.Address2_County).HasMaxLength(100);

                entity.Property(e => e.Address2_Line1).HasMaxLength(100);

                entity.Property(e => e.Address2_Line2).HasMaxLength(100);

                entity.Property(e => e.Address2_Line3).HasMaxLength(100);

                entity.Property(e => e.Address2_Name).HasMaxLength(150);

                entity.Property(e => e.Address2_PostalCode).HasMaxLength(100);

                entity.Property(e => e.Address2_PostOfficeBox).HasMaxLength(100);

                entity.Property(e => e.Address2_PostOfficeBox).HasMaxLength(100);


                entity.Property(e => e.Address3_City).HasMaxLength(100);

                entity.Property(e => e.Address3_Country).HasMaxLength(100);

                entity.Property(e => e.Address3_County).HasMaxLength(100);

                entity.Property(e => e.Address3_Line1).HasMaxLength(100);

                entity.Property(e => e.Address3_Line2).HasMaxLength(100);

                entity.Property(e => e.Address3_Line3).HasMaxLength(100);

                entity.Property(e => e.Address3_Name).HasMaxLength(100);

                entity.Property(e => e.Address3_PostalCode).HasMaxLength(100);

                entity.Property(e => e.Address3_PostOfficeBox).HasMaxLength(100);

                entity.Property(e => e.Address3_PostOfficeBox).HasMaxLength(100);


                entity.Property(e => e.BirthDate).HasColumnType("datetime2");

                entity.Property(e => e.EmailAddress1).HasMaxLength(100);

                entity.Property(e => e.EmailAddress2).HasMaxLength(100);

                entity.Property(e => e.EmailAddress3).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.JobTitle).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.msnfp_LastEventPackageDate).HasColumnType("datetime2");

                entity.Property(e => e.msnfp_LastTransactionDate).HasColumnType("datetime2");

                entity.Property(e => e.msnfp_Sum_LifetimeTransactions).HasColumnType("money");

                entity.Property(e => e.msnfp_UpcomingBirthday).HasColumnType("datetime2");

                entity.Property(e => e.MiddleName).HasMaxLength(100);

                entity.Property(e => e.MobilePhone).HasMaxLength(100);

                entity.Property(e => e.Salutation).HasMaxLength(100);

                entity.Property(e => e.Suffix).HasMaxLength(100);

                entity.Property(e => e.Telephone1).HasMaxLength(100);

                entity.Property(e => e.Telephone2).HasMaxLength(100);

                entity.Property(e => e.Telephone3).HasMaxLength(100);



                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.Property(e => e.msnfp_year0_giving).HasColumnType("money");
                entity.Property(e => e.msnfp_year1_giving).HasColumnType("money");
                entity.Property(e => e.msnfp_year2_giving).HasColumnType("money");
                entity.Property(e => e.msnfp_year3_giving).HasColumnType("money");
                entity.Property(e => e.msnfp_year4_giving).HasColumnType("money");
                entity.Property(e => e.msnfp_lifetimegivingsum).HasColumnType("money");

				entity.HasOne(d => d.msnfp_LastEventPackage)
                    .WithMany(p => p.Contact)
                    .HasForeignKey(d => d.msnfp_LastEventPackageId)
                    .HasConstraintName("FK__Contact__LastEventPackageId__0A688BB6");

                entity.HasOne(d => d.msnfp_LastTransaction)
                    .WithMany(p => p.Contact)
                    .HasForeignKey(d => d.msnfp_LastTransactionId)
                    .HasConstraintName("FK__Contact__LastTransactionId__0A688BB7");

                entity.HasOne(d => d.msnfp_PrimaryMembership)
                    .WithMany(p => p.Contact)
                    .HasForeignKey(d => d.msnfp_PrimaryMembershipId)
                    .HasConstraintName("FK__Contact__PrimaryMembershipId__0A688BB8");

                entity.HasOne(d => d.msnfp_household)
	                .WithMany(p => p.HouseholdMember)
	                .HasForeignKey(d => d.msnfp_householdid)
	                .HasConstraintName("FK__Contact__HouseHold__0A688BB9");

			});

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.Property(e => e.DesignationId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(160);

				entity.Property(e => e.StatusCode).HasColumnName("StatusReason");

                // Deprecated: entity.Property(e => e.DesignationFilter);

                // Deprecated: entity.Property(e => e.DesignationFilterName).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

            });


            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventId)
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CostAmount).HasColumnType("money");

                entity.Property(e => e.ExternalUrl).HasMaxLength(250);

                entity.Property(e => e.Goal).HasColumnType("money");

                entity.Property(e => e.HomePageUrl).HasMaxLength(100);

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.LabelLanguageCode).HasMaxLength(100);

                entity.Property(e => e.LargeImage).HasMaxLength(100);

                entity.Property(e => e.LastPublished).HasColumnType("datetime2");

                entity.Property(e => e.MadeVisible).HasColumnType("datetime2");

                entity.Property(e => e.MapCity).HasMaxLength(100);

                entity.Property(e => e.MapCountry).HasMaxLength(100);

                entity.Property(e => e.MapLine1).HasMaxLength(100);

                entity.Property(e => e.MapLine2).HasMaxLength(100);

                entity.Property(e => e.MapLine3).HasMaxLength(100);

                entity.Property(e => e.MapPostalCode).HasMaxLength(100);

                entity.Property(e => e.MapStateOrProvince).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ProposedEnd).HasColumnType("datetime2");

                entity.Property(e => e.ProposedStart).HasColumnType("datetime2");

                entity.Property(e => e.RemovedOn).HasColumnType("datetime2");

                entity.Property(e => e.SmallImage).HasMaxLength(100);

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK__Event__Designation");

                entity.HasOne(d => d.Configuration)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.ConfigurationId)
                    .HasConstraintName("FK__Event__Configura__00DF2177");

                entity.HasOne(d => d.TermsOfReference)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.TermsOfReferenceId)
                    .HasConstraintName("FK__Event__TermsOfRe__01D345B0");
            });

            modelBuilder.Entity<EventDisclaimer>(entity =>
            {
                entity.HasKey(e => e.EventDisclaimerId);

                entity.Property(e => e.EventDisclaimerId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventDislaimer)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventDisl__Event__14E61A24");
            });

            modelBuilder.Entity<EventDonation>(entity =>
            {
                entity.Property(e => e.EventDonationId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventDonation)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventDona__Event__02C769E9");
            });

            modelBuilder.Entity<EventPackage>(entity =>
            {
                entity.Property(e => e.EventPackageId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountNonReceiptable).HasColumnType("money");

                entity.Property(e => e.AmountReceipted).HasColumnType("money");

                entity.Property(e => e.AmountTax).HasColumnType("money");

                entity.Property(e => e.BillingCity).HasMaxLength(100);

                entity.Property(e => e.BillingCountry).HasMaxLength(100);

                entity.Property(e => e.BillingLine1).HasMaxLength(100);

                entity.Property(e => e.BillingLine2).HasMaxLength(100);

                entity.Property(e => e.BillingLine3).HasMaxLength(100);

                entity.Property(e => e.BillingPostalCode).HasMaxLength(100);

                entity.Property(e => e.BillingStateorProvince).HasMaxLength(100);

                entity.Property(e => e.ChequeNumber).HasMaxLength(100);

                entity.Property(e => e.ChequeWireDate).HasColumnType("datetime2");

                entity.Property(e => e.DataEntryReference).HasMaxLength(100);

                entity.Property(e => e.Date).HasColumnType("datetime2");

                entity.Property(e => e.DateRefunded).HasColumnType("datetime2");

                entity.Property(e => e.Emailaddress1).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.InvoiceIdentifier).HasMaxLength(100);

                entity.Property(e => e.OrganizationName).HasMaxLength(100);

                entity.Property(e => e.RefAmount).HasColumnType("money");

                entity.Property(e => e.RefAmountNonreceiptable).HasColumnType("money");

                entity.Property(e => e.RefAmountReceipted).HasColumnType("money");

                entity.Property(e => e.RefAmountTax).HasColumnType("money");

                entity.Property(e => e.Telephone1).HasMaxLength(100);

                entity.Property(e => e.Telephone2).HasMaxLength(100);

                entity.Property(e => e.ThirdPartyReceipt).HasMaxLength(100);

                entity.Property(e => e.TransactionFraudCode).HasMaxLength(100);

                entity.Property(e => e.TransactionIdentifier).HasMaxLength(100);

                entity.Property(e => e.TransactionResult).HasMaxLength(100);

                entity.Property(e => e.ValDonations).HasColumnType("money");

                entity.Property(e => e.ValProducts).HasColumnType("money");

                entity.Property(e => e.ValSponsorships).HasColumnType("money");

                entity.Property(e => e.ValTickets).HasColumnType("money");

                entity.HasOne(d => d.Configuration)
                    .WithMany(p => p.EventPackage)
                    .HasForeignKey(d => d.ConfigurationId)
                    .HasConstraintName("FK__EventPack__Confi__11158940");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventPackage)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventPack__Event__10216507");

                entity.HasOne(d => d.Paymentmethod)
                    .WithMany(p => p.EventPackages)
                    .HasForeignKey(d => d.PaymentmethodId)
                    .HasConstraintName("FK__EventPack__Payme__1209AD79");
            });

            modelBuilder.Entity<EventProduct>(entity =>
            {
                entity.Property(e => e.EventProductId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountNonReceiptable).HasColumnType("money");

                entity.Property(e => e.AmountReceipted).HasColumnType("money");

                entity.Property(e => e.AmountTax).HasColumnType("money");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SumSold).HasColumnType("money");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventProduct)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventProd__Event__09746778");
            });

            modelBuilder.Entity<EventSponsor>(entity =>
            {
                entity.Property(e => e.EventSponsorId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.LargeImage).HasMaxLength(100);

                entity.Property(e => e.OrderDate).HasColumnType("datetime2");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventSponsor)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventSpon__Event__12FDD1B2");
            });

            modelBuilder.Entity<EventSponsorship>(entity =>
            {
                entity.Property(e => e.EventSponsorshipId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Advantage).HasColumnType("money");

                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.AmountNonReceiptable).HasColumnType("money");
                entity.Property(e => e.AmountReceipted).HasColumnType("money");

				entity.Property(e => e.Date).HasColumnType("datetime2");

                entity.Property(e => e.FromAmount).HasColumnType("money");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SumSold).HasColumnType("money");

                entity.Property((e => e.ValSold)).HasColumnType("money");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventSponsorship)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventSpon__Event__13F1F5EB");
            });

            modelBuilder.Entity<EventTicket>(entity =>
            {
                entity.HasKey(e => e.EvenTicketId);

                entity.Property(e => e.EvenTicketId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountNonReceiptable).HasColumnType("money");

                entity.Property(e => e.AmountReceipted).HasColumnType("money");

                entity.Property(e => e.AmountTax).HasColumnType("money");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ValTickets).HasColumnType("money");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventTicket)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventTick__Event__0697FACD");
            });

            modelBuilder.Entity<GiftAidDeclaration>(entity =>
            {
                entity.Property(e => e.GiftAidDeclarationId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeclarationDate).HasColumnType("datetime2");

                entity.Property(e => e.DeclarationDelivered).HasMaxLength(100);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(150);

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.Property(e => e.Updated).HasColumnType("datetime2");
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.Property(e => e.MembershipId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime2");

                entity.Property(e => e.Name)
                    .HasMaxLength(100);

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime2");

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.HasOne(d => d.MembershipCategory)
                    .WithMany(p => p.Membership)
                    .HasForeignKey(d => d.MembershipCategoryId)
                    .HasConstraintName("FK__Membershi__Membe__70A8B9AE");
            });

            modelBuilder.Entity<MembershipCategory>(entity =>
            {
                entity.Property(e => e.MembershipCategoryId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountMembership).HasColumnType("money");

                entity.Property(e => e.AmountTax).HasColumnType("money");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.GoodWillDate).HasColumnType("datetime2");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.RenewalDate).HasColumnType("datetime2");

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");
            });

            modelBuilder.Entity<MembershipGroup>(entity =>
            {
                entity.Property(e => e.MembershipGroupId)
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.GroupName).HasMaxLength(100);

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");
            });

            modelBuilder.Entity<MembershipOrder>(entity =>
            {
                entity.Property(e => e.MembershipOrderId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.OrderDate).HasColumnType("datetime2");

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.HasOne(d => d.FromMembershipCategory)
                    .WithMany(p => p.MembershipOrder)
                    .HasForeignKey(d => d.FromMembershipCategoryId)
                    .HasConstraintName("FK__Membershi__FromM__6EC0713C");

                entity.HasOne(d => d.ToMembershipGroup)
                    .WithMany(p => p.MembershipOrders)
                    .HasForeignKey(d => d.ToMembershipGroupId)
                    .HasConstraintName("FK__Membershi__ToMem__6FB49575");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.PaymentMethodId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BankActNumber).HasMaxLength(100);

                entity.Property(e => e.BankActRtNumber).HasMaxLength(100);

                entity.Property(e => e.BankName).HasMaxLength(100);

                entity.Property(e => e.BillingCity).HasMaxLength(100);

                entity.Property(e => e.BillingCountry).HasMaxLength(100);

                entity.Property(e => e.BillingLine1).HasMaxLength(100);

                entity.Property(e => e.BillingLine2).HasMaxLength(100);

                entity.Property(e => e.BillingLine3).HasMaxLength(100);

                entity.Property(e => e.BillingPostalCode).HasMaxLength(100);

                entity.Property(e => e.BillingStateorProvince).HasMaxLength(100);

                entity.Property(e => e.CcExpMmYy).HasMaxLength(100);

                entity.Property(e => e.CcLast4).HasMaxLength(100);

                entity.Property(e => e.CcExpDate).HasColumnType("datetime2");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.Emailaddress1).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NameOnFile).HasMaxLength(100);

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.Property(e => e.Telephone1).HasMaxLength(100);

                entity.Property(e => e.TransactionFraudCode).HasMaxLength(100);

                entity.Property(e => e.TransactionIdentifier).HasMaxLength(100);

                entity.Property(e => e.TransactionResult).HasMaxLength(100);

                entity.Property(e => e.AbaFinancialInstitutionName).HasMaxLength(3);
                
                entity.Property(e => e.NameAsItAppearsOnTheAccount).HasMaxLength(100);

                entity.HasOne(d => d.PaymentProcessor)
                    .WithMany(p => p.PaymentMethod)
                    .HasForeignKey(d => d.PaymentProcessorId)
                    .HasConstraintName("FK__PaymentMe__Payme__56E8E7AB");
            });

            modelBuilder.Entity<PaymentProcessor>(entity =>
            {
                entity.Property(e => e.PaymentProcessorId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);


                entity.Property(e => e.AdyenMerchantAccount).HasMaxLength(100);
                entity.Property(e => e.AdyenUsername).HasMaxLength(100);
                entity.Property(e => e.AdyenPassword).HasMaxLength(100);
                entity.Property(e => e.AdyenCheckoutUrl).HasMaxLength(100);

                entity.Property(e => e.IatsAgentCode).HasMaxLength(100);
                entity.Property(e => e.IatsPassword).HasMaxLength(100);

                entity.Property(e => e.MonerisStoreId).HasMaxLength(100);
                entity.Property(e => e.MonerisApiKey).HasMaxLength(100);

                entity.Property(e => e.StripeServiceKey).HasMaxLength(100);

                entity.Property(e => e.WorldPayServiceKey).HasMaxLength(100);

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.Property(e => e.Updated).HasColumnType("datetime2");

                entity.Property(e => e.ScotiabankCustomerNumber).HasMaxLength(10);

                entity.Property(e => e.BmoOriginatorId).HasMaxLength(10);

                entity.Property(e => e.OriginatorShortName).HasMaxLength(15);

                entity.Property(e => e.OriginatorLongName).HasMaxLength(30);

                entity.Property(e => e.AbaRemitterName).HasMaxLength(16);

                entity.Property(e => e.AbaUserName).HasMaxLength(26);

                entity.Property(e => e.AbaUserNumber).HasMaxLength(6);

            });

            modelBuilder.Entity<PaymentSchedule>(entity =>
            {
                entity.Property(e => e.PaymentScheduleId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AmountMembership).HasColumnType("money");

                entity.Property(e => e.AmountNonReceiptable).HasColumnType("money");

                entity.Property(e => e.AmountReceipted).HasColumnType("money");

                entity.Property(e => e.AmountTax).HasColumnType("money");

                entity.Property(e => e.Appraiser).HasMaxLength(100);

                entity.Property(e => e.BillingCity).HasMaxLength(100);

                entity.Property(e => e.BillingCountry).HasMaxLength(100);

                entity.Property(e => e.BillingLine1).HasMaxLength(100);

                entity.Property(e => e.BillingLine2).HasMaxLength(100);

                entity.Property(e => e.BillingLine3).HasMaxLength(100);

                entity.Property(e => e.BillingPostalCode).HasMaxLength(100);

                entity.Property(e => e.BillingStateorProvince).HasMaxLength(100);

                entity.Property(e => e.BookDate).HasColumnType("datetime2");

                entity.Property(e => e.CancelledOn).HasColumnType("datetime2");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DataEntryReference).HasMaxLength(100);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.DepositDate).HasColumnType("datetime2");

                entity.Property(e => e.EmailAddress1).HasMaxLength(100);

                entity.Property(e => e.EndonDate).HasColumnType("datetime2");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.FirstPaymentDate).HasColumnType("datetime2");

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.LastPaymentDate).HasColumnType("datetime2");

                entity.Property(e => e.InvoiceIdentifier).HasMaxLength(100);

                entity.Property(e => e.MobilePhone).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NextPaymentDate).HasColumnType("datetime2");

                entity.Property(e => e.OrganizationName).HasMaxLength(100);

                entity.Property(e => e.RecurringAmount).HasColumnType("money");

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.Property(e => e.Telephone1).HasMaxLength(100);

                entity.Property(e => e.Telephone2).HasMaxLength(100);

                entity.Property(e => e.TransactionDescription).HasMaxLength(100);

                entity.Property(e => e.TransactionFraudCode).HasMaxLength(100);

                entity.Property(e => e.TransactionIdentifier).HasMaxLength(100);

                entity.Property(e => e.TransactionResult).HasMaxLength(100);

                entity.Property(e => e.TributeAcknowledgement).HasMaxLength(100);

                entity.HasOne(d => d.Configuration)
                    .WithMany(p => p.PaymentSchedule)
                    .HasForeignKey(d => d.ConfigurationId)
                    .HasConstraintName("FK__PaymentSc__Confi__73852659");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.PaymentSchedule)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__PaymentSc__Event__756D6ECB");

                entity.HasOne(d => d.EventPackage)
                    .WithMany(p => p.PaymentSchedule)
                    .HasForeignKey(d => d.EventPackageId)
                    .HasConstraintName("FK__PaymentSc__Event__76619304");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.PaymentSchedules)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK__PaymentSc__Payme__719CDDE7");

                entity.HasOne(d => d.MembershipCategory)
                    .WithMany(p => p.PaymentSchedules)
                    .HasForeignKey(d => d.MembershipCategoryId)
                    .HasConstraintName("FK__PaymentSc_To_MembershipCategory");

                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.PaymentSchedules);
                //.HasForeignKey(d => d.MembershipId)
                //.HasConstraintName("FK__PaymentSc_To_Membership");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountNonreceiptable).HasColumnType("money");

                entity.Property(e => e.AmountReceipted).HasColumnType("money");

                entity.Property(e => e.AmountTax).HasColumnType("money");

                entity.Property(e => e.Date).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Product__EventId__078C1F06");

                entity.HasOne(d => d.EventPackage)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.EventPackageId)
                    .HasConstraintName("FK__Product__EventPa__0880433F");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.Property(e => e.ReceiptId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountNonReceiptable).HasColumnType("money");

                entity.Property(e => e.AmountReceipted).HasColumnType("money");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.ExpectedTaxCredit).HasColumnType("money");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.LastDonationDate).HasColumnType("datetime2");

                entity.Property(e => e.ReceiptIssueDate).HasColumnType("datetime2");

                entity.Property(e => e.ReceiptNumber).HasMaxLength(100);

                entity.Property(e => e.ReceiptStatus).HasMaxLength(100);

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.Property(e => e.Printed).HasColumnType("datetime2");

                entity.HasOne(d => d.PaymentSchedule)
                    .WithMany(p => p.Receipt)
                    .HasForeignKey(d => d.PaymentScheduleId)
                    .HasConstraintName("FK__Receipt__Payment__7849DB76");

                entity.HasOne(d => d.ReceiptStack)
                    .WithMany(p => p.Receipt)
                    .HasForeignKey(d => d.ReceiptStackId)
                    .HasConstraintName("FK__Receipt__Receipt__7755B73D");

                entity.HasOne(d => d.ReplacesReceipt)
                    .WithMany(p => p.InverseReplacesReceipt)
                    .HasForeignKey(d => d.ReplacesReceiptId)
                    .HasConstraintName("FK__Receipt__Replace__793DFFAF");
            });

            modelBuilder.Entity<ReceiptLog>(entity =>
            {
                entity.Property(e => e.ReceiptLogId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.EntryBy).HasMaxLength(100);

                entity.Property(e => e.EntryReason).HasMaxLength(100);

                entity.Property(e => e.ReceiptNumber).HasMaxLength(100);

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.HasOne(d => d.ReceiptStack)
                    .WithMany(p => p.ReceiptLog)
                    .HasForeignKey(d => d.ReceiptStackId)
                    .HasConstraintName("FK__ReceiptLo__Recei__7A3223E8");
            });

            //modelBuilder.Entity<ReceiptEmailLog>();

            modelBuilder.Entity<ReceiptStack>(entity =>
            {
                entity.Property(e => e.ReceiptStackId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.Prefix).HasMaxLength(100);

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.HasOne(d => d.Configuration)
                    .WithMany(p => p.ReceiptStack)
                    .HasForeignKey(d => d.ConfigurationId)
                    .HasConstraintName("FK__ReceiptSt__Confi__7B264821");
            });

            modelBuilder.Entity<Refund>(entity =>
            {
                entity.Property(e => e.RefundId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountMembership).HasColumnType("money");

                entity.Property(e => e.AmountNonReceiptable).HasColumnType("money");

                entity.Property(e => e.AmountReceipted).HasColumnType("money");

                entity.Property(e => e.AmountTax).HasColumnType("money");

                entity.Property(e => e.BookDate).HasColumnType("datetime2");

                entity.Property(e => e.ChequeNumber).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.ReceivedDate).HasColumnType("datetime2");

                entity.Property(e => e.RefAmount).HasColumnType("money");

                entity.Property(e => e.RefAmountMembership).HasColumnType("money");

                entity.Property(e => e.RefAmountNonreceiptable).HasColumnType("money");

                entity.Property(e => e.RefAmountReceipted).HasColumnType("money");

                entity.Property(e => e.RefAmountTax).HasColumnType("money");

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.Property(e => e.TransactionIdentifier).HasMaxLength(100);

                entity.Property(e => e.TransactionResult).HasMaxLength(100);

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Refund)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK__Refund__Transact__7C1A6C5A");
            });

            modelBuilder.Entity<PreferenceCategory>(entity =>
            {
                entity.Property(e => e.preferencecategoryid).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Preference>(entity =>
            {
                entity.Property(e => e.preferenceid).HasDefaultValueSql("(newid())");
                entity.Property(e => e.name).HasMaxLength(150);

                entity.HasOne(d => d.PreferenceCategory)
                    .WithMany(p => p.Preference)
                    .HasForeignKey(d => d.preferencecategoryid)
                    .HasConstraintName("FK__Preference__PreferenceCategory__18B6AB51");
            });

            modelBuilder.Entity<EventPreference>(entity =>
            {
                entity.Property(e => e.EventPreferenceId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.HasOne(d => d.PreferenceCategory)
                    .WithMany(p => p.EventPreference)
                    .HasForeignKey(d => d.PreferenceCategoryId)
                    .HasConstraintName("FK__EventPreference__PreferenceCategory__18B6AB52");
            });

            modelBuilder.Entity<RegistrationPreference>(entity =>
            {
                entity.Property(e => e.RegistrationPreferenceId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Other).HasMaxLength(150);
            });

            modelBuilder.Entity<EventTable>(entity =>
            {
                entity.Property(e => e.EventTableId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Identifier).HasMaxLength(100);
                entity.Property(e => e.TableCapacity).HasMaxLength(100);
                entity.Property(e => e.TableNumber).HasMaxLength(100);
            });

			modelBuilder.Entity<Payment>(entity =>
			{
				entity.Property(e => e.PaymentId).HasDefaultValueSql("(newid())");
				entity.Property(e => e.TransactionFraudCode).HasMaxLength(100);
				entity.Property(e => e.TransactionIdentifier).HasMaxLength(100);
				entity.Property(e => e.TransactionResult).HasMaxLength(100);
				entity.Property(e => e.InvoiceIdentifier).HasMaxLength(100);
				entity.Property(e => e.ChequeNumber).HasMaxLength(100);
				entity.Property(e => e.Name).HasMaxLength(100);
				entity.Property(e => e.Amount).HasColumnType("money");
				entity.Property(e => e.AmountRefunded).HasColumnType("money");
				entity.Property(e => e.AmountBalance).HasColumnType("money");
			});

			modelBuilder.Entity<Registration>(entity =>
            {
                entity.Property(e => e.RegistrationId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BillingCity).HasMaxLength(100);

                entity.Property(e => e.BillingCountry).HasMaxLength(100);

                entity.Property(e => e.BillingLine1).HasMaxLength(100);

                entity.Property(e => e.BillingLine2).HasMaxLength(100);

                entity.Property(e => e.BillingLine3).HasMaxLength(100);

                entity.Property(e => e.BillingPostalCode).HasMaxLength(100);

                entity.Property(e => e.BillingStateorProvince).HasMaxLength(100);

                entity.Property(e => e.Date).HasColumnType("datetime2");

                entity.Property(e => e.Emailaddress1).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Telephone1).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.Telephone).HasMaxLength(100);

                entity.Property(e => e.Address_Line1).HasMaxLength(100);

                entity.Property(e => e.Address_Line2).HasMaxLength(100);

                entity.Property(e => e.Address_City).HasMaxLength(100);

                entity.Property(e => e.Address_Province).HasMaxLength(20);

                entity.Property(e => e.Address_PostalCode).HasMaxLength(100);

                entity.Property(e => e.Address_Country).HasMaxLength(100);

                entity.Property(e => e.Team).HasMaxLength(150);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Registration)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Registrat__Event__18B6AB08");

                entity.HasOne(d => d.EventPackage)
                    .WithMany(p => p.Registration)
                    .HasForeignKey(d => d.EventPackageId)
                    .HasConstraintName("FK__Registrat__Event__19AACF41");

                entity.HasOne(d => d.EventTicket)
                    .WithMany(p => p.Registration)
                    .HasForeignKey(d => d.EventTicketId)
                    .HasConstraintName("FK__Registrat__Event__1B9317B3");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Registration)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK__Registrat__Ticke__1A9EF37A");
            });

            modelBuilder.Entity<RelatedImage>(entity =>
            {
                entity.Property(e => e.RelatedImageId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.LastPublished).HasColumnType("datetime2");

                entity.Property(e => e.SmallImage).HasMaxLength(100);

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");
            });

            modelBuilder.Entity<Response>(entity =>
            {
                entity.Property(e => e.ResponseId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.HasOne(d => d.PaymentSchedule)
                    .WithMany(p => p.Response)
                    .HasForeignKey(d => d.PaymentScheduleId)
                    .HasConstraintName("FK__Response__Paymen__7EF6D905");

                entity.HasOne(d => d.RegistrationPackage)
                    .WithMany(p => p.Response)
                    .HasForeignKey(d => d.RegistrationPackageId)
                    .HasConstraintName("FK__Response__Regist__7FEAFD3E");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Response)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK__Response__Transa__7E02B4CC");
            });

            modelBuilder.Entity<Sponsorship>(entity =>
            {
                entity.Property(e => e.SponsorshipId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountNonreceiptable).HasColumnType("money");

                entity.Property(e => e.AmountReceipted).HasColumnType("money");

                entity.Property(e => e.AmountTax).HasColumnType("money");

                entity.Property(e => e.Date).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.SponsorshipNavigation)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Sponsorsh__Event__15DA3E5D");

                entity.HasOne(d => d.EventPackage)
                    .WithMany(p => p.Sponsorship)
                    .HasForeignKey(d => d.EventPackageId)
                    .HasConstraintName("FK__Sponsorsh__Event__16CE6296");

                entity.HasOne(d => d.EventSponsorship)
	                .WithMany(p => p.Sponsorship)
	                .HasForeignKey(d => d.EventSponsorshipId)
	                .HasConstraintName("FK__Sponsorsh__Event__17C286CF");
            });

            //modelBuilder.Entity<TermsOfReference>(entity =>
            //{
            //    entity.Property(e => e.TermsOfReferenceId).HasDefaultValueSql("(newid())");

            //    entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

            //    entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

            //    entity.Property(e => e.Identifier).HasMaxLength(100);

            //    entity.Property(e => e.PrivacyUrl).HasMaxLength(100);

            //    entity.Property(e => e.SyncDate).HasColumnType("datetime2");

            //    entity.Property(e => e.TermsConditionsUrl).HasMaxLength(100);
            //});

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.TicketId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountNonreceiptable).HasColumnType("money");

                entity.Property(e => e.AmountReceipted).HasColumnType("money");

                entity.Property(e => e.AmountTax).HasColumnType("money");

                entity.Property(e => e.Date).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Ticket__EventId__0A688BB1");

                entity.HasOne(d => d.EventPackage)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.EventPackageId)
                    .HasConstraintName("FK__Ticket__EventPac__0B5CAFEA");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountMembership).HasColumnType("money");

                entity.Property(e => e.AmountNonReceiptable).HasColumnType("money");

                entity.Property(e => e.AmountReceipted).HasColumnType("money");

                entity.Property(e => e.AmountTax).HasColumnType("money");

                entity.Property(e => e.AmountTransfer).HasColumnType("money");

                entity.Property(e => e.Appraiser).HasMaxLength(100);

                entity.Property(e => e.BillingCity).HasMaxLength(100);

                entity.Property(e => e.BillingCountry).HasMaxLength(100);

                entity.Property(e => e.BillingLine1).HasMaxLength(100);

                entity.Property(e => e.BillingLine2).HasMaxLength(100);

                entity.Property(e => e.BillingLine3).HasMaxLength(100);

                entity.Property(e => e.BillingPostalCode).HasMaxLength(100);

                entity.Property(e => e.BillingStateorProvince).HasMaxLength(100);

                entity.Property(e => e.BookDate).HasColumnType("datetime2");

                entity.Property(e => e.ChequeNumber).HasMaxLength(100);

                entity.Property(e => e.ChequeWireDate).HasColumnType("datetime2");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DataEntryReference).HasMaxLength(100);

                entity.Property(e => e.DateRefunded).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.Emailaddress1).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.GaAmountClaimed).HasColumnType("money");

                entity.Property(e => e.LastFailedRetry).HasColumnType("datetime2");

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.InvoiceIdentifier).HasMaxLength(100);

                entity.Property(e => e.MobilePhone).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NextFailedRetry).HasColumnType("datetime2");

                entity.Property(e => e.OrganizationName).HasMaxLength(100);

                entity.Property(e => e.ReceivedDate).HasColumnType("datetime2");

                entity.Property(e => e.RefAmount).HasColumnType("money");

                entity.Property(e => e.RefAmountMembership).HasColumnType("money");

                entity.Property(e => e.RefAmountNonreceiptable).HasColumnType("money");

                entity.Property(e => e.RefAmountReceipted).HasColumnType("money");

                entity.Property(e => e.RefAmountTax).HasColumnType("money");

                entity.Property(e => e.ReturnedDate).HasColumnType("datetime2");

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.Property(e => e.Telephone1).HasMaxLength(100);

                entity.Property(e => e.Telephone2).HasMaxLength(100);

                entity.Property(e => e.ThirdPartyReceipt).HasMaxLength(100);

                entity.Property(e => e.TransactionDescription).HasMaxLength(100);

                entity.Property(e => e.TransactionFraudCode).HasMaxLength(100);

                entity.Property(e => e.TransactionIdentifier).HasMaxLength(100);

                entity.Property(e => e.TransactionNumber).HasMaxLength(255);

                entity.Property(e => e.TransactionResult).HasMaxLength(100);

                entity.Property(e => e.TributeAcknowledgement).HasMaxLength(100);

                entity.Property(e => e.ValidationDate).HasColumnType("datetime2");

				entity.Property(e => e.StatusCode).HasColumnName("StatusReason");


                entity.HasOne(d => d.MembershipCategory)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.MembershipId)
                    .HasConstraintName("FK__Transacti__Membe__625A9A58");

                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.MembershipInstanceId)
                    .HasConstraintName("FK__Transacti__Membe__625A9A59");

                entity.HasOne(d => d.Configuration)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.ConfigurationId)
                    .HasConstraintName("FK__Transacti__Confi__625A9A57");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Transacti__Event__65370702");

                entity.HasOne(d => d.TransactionPaymentMethod)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TransactionPaymentMethodId)
                    .HasConstraintName("FK__Transacti__trans__6166761E");

                entity.HasOne(d => d.TransactionPaymentSchedule)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.TransactionPaymentScheduleId)
                    .HasConstraintName("FK__Transacti__trans__634EBE90");
            });

            modelBuilder.Entity<TributeOrMemory>(entity =>
            {
                entity.HasKey(e => e.TributeOrMemoryId);

                entity.Property(e => e.TributeOrMemoryId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SyncDate).HasColumnType("datetime2");

                entity.Property(e => e.Identifier).HasMaxLength(100);

            });

            modelBuilder.Entity<DonorCommitment>(entity =>
            {
	            entity.HasKey(e => e.DonorCommitmentId);
				entity.Property(e=>e.DonorCommitmentId).HasDefaultValueSql("(newid())");
				entity.Property(e => e.CreatedOn).HasColumnType("datetime2");
				entity.Property(e => e.DeletedDate).HasColumnType("datetime2");
				entity.Property(e => e.SyncDate).HasColumnType("datetime2");
				entity.Property(e => e.TotalAmount).HasColumnType("money");
				entity.Property(e => e.BookDate).HasColumnType("datetime2");
				entity.Property(e => e.TotalAmountBalance).HasColumnType("money");

			});

			modelBuilder.Entity<SyncLog>().ToTable("SyncException");

            //modelBuilder.Entity<PageOrder>(entity =>
            //{
            //    entity.HasKey(e => e.PageOrderId);

            //    entity.Property(e => e.PageOrderId).HasDefaultValueSql("newid()");

            //    entity.Property(e => e.CreatedOn).HasColumnType("datetime2");

            //    entity.Property(e => e.DeletedDate).HasColumnType("datetime2");

            //    entity.Property(e => e.OrderDate).HasColumnType("datetime2");

            //    entity.Property(e => e.Title).HasMaxLength(150);

            //    entity.HasOne(d => d.FromDonationList)
            //        .WithMany(p => p.FromDonationList_PageOrders)
            //        .HasForeignKey(d => d.FromDonationListId)
            //        .HasConstraintName("FK__PageOrder__FromDonationList__00000000");

            //    entity.HasOne(d => d.ToDonationList)
            //        .WithMany(p => p.ToDonationList_PageOrders)
            //        .HasForeignKey(d => d.ToDonationListId)
            //        .HasConstraintName("FK__PageOrder__ToDonationList__00000002");


            //    entity.HasOne(d => d.FromDonationPage)
            //        .WithMany(p => p.PageOrders)
            //        .HasForeignKey(d => d.FromDonationPageId)
            //        .HasConstraintName("FK__PageOrder__FromDonationPage__00000001");

            //});

            //modelBuilder.Entity<Note>(entity =>
            //{
            //    entity.HasKey(e => e.NoteId);
            //    entity.Property(e => e.NoteId).HasDefaultValueSql("newid()");
            //    //entity.Property(e => e.Document).HasMaxLength(4000);
            //    entity.Property(e => e.FileName).HasMaxLength(255);
            //    entity.Property(e => e.Description).HasMaxLength(100000);
            //    entity.Property(e => e.Title).HasMaxLength(500);
            //    entity.Property(e => e.MimeType).HasMaxLength(256);

            //    entity.HasOne(d => d.RegardingObject)
            //        .WithMany(p => p.Note)
            //        .HasForeignKey(d => d.RegardingObjectId)
            //        .HasConstraintName("FK__Note__RegardingObjectId_BankRun__0A688BB5");

            //});
        }
    }
}
