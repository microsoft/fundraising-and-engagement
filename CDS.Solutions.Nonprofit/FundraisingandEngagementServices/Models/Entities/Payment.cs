using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_Payment")]
    public class Payment : PaymentEntity
    {
		[Key]
        public Guid PaymentId { get; set; }

        [ForeignKey(nameof(PaymentProcessor))]
        public Guid? PaymentProcessorId { get; set; }

        [ForeignKey(nameof(PaymentMethod))]
        public Guid? PaymentMethodId { get; set; }

        [ForeignKey(nameof(EventPackage))]
        public Guid? EventPackageId { get; set; }

        public Guid? CustomerId { get; set; }

		public Guid? ResponseId { get; set; }

		public Guid? ConfigurationId { get; set; }

		public decimal? Amount { get; set; }

        public decimal? AmountRefunded { get; set; }

        public decimal? AmountBalance { get; set; }

        public string TransactionFraudCode { get; set; }

        public string TransactionIdentifier { get; set; }

        public string TransactionResult { get; set; }

        public string ChequeNumber { get; set; }

        public string Name { get; set; }

        public int? PaymentType { get; set; }

        public int? CcBrandCodePayment { get; set; }

		public string InvoiceIdentifier { get; set; }

		public DateTime? DateRefunded { get; set; }

		public virtual PaymentProcessor PaymentProcessor { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual EventPackage EventPackage { get; set; }
    }
}
