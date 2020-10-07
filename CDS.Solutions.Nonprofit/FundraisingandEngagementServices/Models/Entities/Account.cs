using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Enums;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("account")]
	public class Account : PaymentEntity, ICustomer
    {
		[EntityNameMap("accountid")]
        public Guid AccountId { get; set; }
        public Guid CustomerId => AccountId;

		//[EntityLogicalName("customeraddress")]
		//[EntityReferenceMap("Address1_AddressId")]
        public Guid? Address1_AddressId { get; set; }
		//[EntityLogicalName("customeraddress")]
		//[EntityReferenceMap("Address2_AddressId")]
        public Guid? Address2_AddressId { get; set; }
		//[EntityLogicalName("account")]
		//[EntityReferenceMap("MasterId")]
        public Guid? MasterId { get; set; }
		//[EntityLogicalName("businessunit")]
		//[EntityReferenceMap("OwningBusinessUnit")]
        public Guid? OwningBusinessUnitId { get; set; }
        //[EntityLogicalName("msnfp_givinglevelinstance")]
        //[EntityReferenceMap("msnfp_givinglevelid")]
        public Guid? msnfp_GivingLevelId { get; set; }
        //[EntityLogicalName("msnfp_eventpackage")]
        //[EntityReferenceMap("msnfp_lasteventpackageid")]
        public Guid? msnfp_LastEventPackageId { get; set; }
		//[EntityLogicalName("msnfp_transaction")]
		//[EntityReferenceMap("msnfp_lasttransactionid")]
        public Guid? msnfp_LastTransactionId { get; set; }
		//[EntityLogicalName("msnfp_membership")]
		//[EntityReferenceMap("msnfp_primarymembershipid")]
        public Guid? msnfp_PrimaryMembershipId { get; set; }
		//[EntityLogicalName("account")]
		//[EntityReferenceMap("ParentAccountId")]
        public Guid? ParentAccountId { get; set; }
		//[EntityLogicalName("transactioncurrency")]
		//[EntityReferenceMap("TransactionCurrencyId")]
        public Guid? TransactionCurrencyId { get; set; }

        [EntityOptionSetMap("address1_addresstypecode")]
		public int? Address1_AddressTypeCode { get; set; }
		[EntityNameMap("address1_city")]
        public string Address1_City { get; set; }
		[EntityNameMap("address1_country")]
        public string Address1_Country { get; set; }
		[EntityNameMap("address1_county")]
        public string Address1_County { get; set; }
		[EntityNameMap("address1_latitude")]
        public float? Address1_Latitude { get; set; }
		[EntityNameMap("address1_line1")]
        public string Address1_Line1 { get; set; }
		[EntityNameMap("address1_line2")]
        public string Address1_Line2 { get; set; }
		[EntityNameMap("address1_line3")]
        public string Address1_Line3 { get; set; }
		[EntityNameMap("address1_longitude")]
        public float? Address1_Longitude { get; set; }
		[EntityNameMap("address1_name")]
        public string Address1_Name { get; set; }
		[EntityNameMap("address1_postalcode")]
        public string Address1_PostalCode { get; set; }
		[EntityNameMap("address1_postofficebox")]
        public string Address1_PostOfficeBox { get; set; }
		[EntityNameMap("address1_stateorprovince")]
        public string Address1_StateOrProvince { get; set; }
		
        [EntityOptionSetMap("address2_addresstypecode")]
        public int? Address2_AddressTypeCode { get; set; }
		[EntityNameMap("address2_city")]
        public string Address2_City { get; set; }
		[EntityNameMap("address2_country")]
        public string Address2_Country { get; set; }
		[EntityNameMap("address2_county")]
        public string Address2_County { get; set; }
		[EntityNameMap("address2_latitude")]
        public float? Address2_Latitude { get; set; }
		[EntityNameMap("address2_line1")]
        public string Address2_Line1 { get; set; }
		[EntityNameMap("address2_line2")]
        public string Address2_Line2 { get; set; }
		[EntityNameMap("address2_line3")]
        public string Address2_Line3 { get; set; }
		[EntityNameMap("address2_longitude")]
        public float? Address2_Longitude { get; set; }
		[EntityNameMap("address2_name")]
        public string Address2_Name { get; set; }
		[EntityNameMap("address2_postalcode")]
        public string Address2_PostalCode { get; set; }
		[EntityNameMap("address2_postofficebox")]
        public string Address2_PostOfficeBox { get; set; }
		[EntityNameMap("address2_stateorprovince")]
        public string Address2_StateOrProvince { get; set; }

        [EntityNameMap("donotbulkemail")]
        public bool? DoNotBulkEMail { get; set; }
		[EntityNameMap("donotbulkpostalmail")]
        public bool? DoNotBulkPostalMail { get; set; }
		[EntityNameMap("donotemail")]
        public bool? DoNotEmail { get; set; }
		[EntityNameMap("donotfax")]
        public bool? DoNotFax { get; set; }
		[EntityNameMap("donotphone")]
        public bool? DoNotPhone { get; set; }
		[EntityNameMap("donotpostalmail")]
        public bool? DoNotPostalMail { get; set; }
		[EntityNameMap("donotsendmm")]
        public bool? DoNotSendMM { get; set; }
		[EntityNameMap("emailaddress1")]
        public string EmailAddress1 { get; set; }
		[EntityNameMap("emailaddress2")]
        public string EmailAddress2 { get; set; }
		[EntityNameMap("emailaddress3")]
        public string EmailAddress3 { get; set; }

        [EntityNameMap("msnfp_anonymity")]
        public int? msnfp_Anonymity { get; set; }
		[EntityNameMap("msnfp_count_lifetimetransactions")]
        public int? msnfp_Count_LifetimeTransactions { get; set; }
		[EntityNameMap("msnfp_lasteventpackagedate", Format = "yyyy-MM-dd")]
        public DateTime? msnfp_LastEventPackageDate { get; set; }
		[EntityNameMap("msnfp_lasttransactiondate", Format = "yyyy-MM-dd")]
        public DateTime? msnfp_LastTransactionDate { get; set; }
		[EntityOptionSetMap("msnfp_preferredlanguagecode")]
        public int? msnfp_PreferredLanguageCode { get; set; }
		[EntityOptionSetMap("msnfp_receiptpreferencecode")]
        public int? msnfp_ReceiptPreferenceCode { get; set; }
        [Column(TypeName = "money")]
		[EntityNameMap("msnfp_sum_lifetimetransactions")]
        public decimal? msnfp_Sum_LifetimeTransactions { get; set; }
		[EntityOptionSetMap("msnfp_telephone1typecode")]
        public int? msnfp_Telephone1TypeCode { get; set; }
        [EntityOptionSetMap("msnfp_telephone2typecode")]
        public int? msnfp_Telephone2TypeCode { get; set; }
        [EntityOptionSetMap("msnfp_telephone3typecode")]
        public int? msnfp_Telephone3TypeCode { get; set; }
		[EntityNameMap("msnfp_vip")]
        public bool? msnfp_Vip { get; set; }

		[EntityNameMap("merged")]
        public bool? Merged { get; set; }
		[EntityNameMap("name")]
        public string Name { get; set; }
		[EntityNameMap("telephone1")]
        public string Telephone1 { get; set; }
		[EntityNameMap("telephone2")]
        public string Telephone2 { get; set; }
		[EntityNameMap("telephone3")]
        public string Telephone3 { get; set; }
		[EntityNameMap("websiteurl")]
        public string WebSiteURL { get; set; }

        [EntityOptionSetMap("msnfp_accounttype")]
		public AccountType? msnfp_accounttype { get; set; }

		[EntityOptionSetMap("msnfp_year0_category")]
		public DonorType? msnfp_year0_category { get; set; }
		[Column(TypeName = "money")]
		[EntityNameMap("msnfp_year0_giving")]
		public decimal? msnfp_year0_giving { get; set; }
		[EntityOptionSetMap("msnfp_year1_category")]
		public DonorType? msnfp_year1_category { get; set; }
		[Column(TypeName = "money")]
		[EntityNameMap("msnfp_year1_giving")]
		public decimal? msnfp_year1_giving { get; set; }
		[EntityOptionSetMap("msnfp_year2_category")]
		public DonorType? msnfp_year2_category { get; set; }
		[Column(TypeName = "money")]
		[EntityNameMap("msnfp_year2_giving")]
		public decimal? msnfp_year2_giving { get; set; }
		[EntityOptionSetMap("msnfp_year3_category")]
		public DonorType? msnfp_year3_category { get; set; }
		[Column(TypeName = "money")]
		[EntityNameMap("msnfp_year3_giving")]
		public decimal? msnfp_year3_giving { get; set; }
		[EntityOptionSetMap("msnfp_year4_category")]
		public DonorType? msnfp_year4_category { get; set; }
		[Column(TypeName = "money")]
		[EntityNameMap("msnfp_year4_giving")]
		public decimal? msnfp_year4_giving { get; set; }

		[Column(TypeName = "money")]
		[EntityNameMap("msnfp_lifetimegivingsum")]
		public decimal? msnfp_lifetimegivingsum { get; set; }

		// todo: check on these
		[EntityOptionSetMap("msnfp_classificationcode")]
		public int? msnfp_classificationcode { get; set; }
		[EntityNameMap("msnfp_issolicitor")]
        public bool? msnfp_issolicitor { get; set; }
		[EntityNameMap("msnfp_signup")]
        public bool? msnfp_signup { get; set; }
		[EntityNameMap("msnfp_solicitcode")]
        public bool? msnfp_solicitcode { get; set; }
		[EntityNameMap("msnfp_solicitdate", Format = "yyyy-MM-dd")]
        public DateTime? msnfp_solicitdate { get; set; }
		[EntityNameMap("msnfp_vendorid")]
        public string msnfp_vendorid { get; set; }

        // end todo: check on these



        public virtual TransactionCurrency TransactionCurrency { get; set; }
        public virtual EventPackage msnfp_LastEventPackage { get; set; }
        public virtual Transaction msnfp_LastTransaction { get; set; }
        public virtual Membership msnfp_PrimaryMembership { get; set; }
        public virtual Account ParentAccount { get; set; }
        public virtual ICollection<Account> ChildAccount { get; set; }
		public virtual ICollection<Contact> HouseholdMember { get; set; }
    }
}
