using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("TransactionCurrency")]
    public class TransactionCurrency : PaymentEntity
    {
        [EntityNameMap("TransactionCurrencyId")]
        public Guid TransactionCurrencyId { get; set; }

        [MaxLength(150)]
        [EntityNameMap("CurrencyName")]
        public string CurrencyName { get; set; }

        [MaxLength(150)]
        [EntityNameMap("CurrencySymbol")]
        public string CurrencySymbol { get; set; }

        [MaxLength(150)]
        [EntityNameMap("IsoCurrencyCode")]
        public string IsoCurrencyCode { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        [EntityNameMap("ExchangeRate")]
        public decimal? ExchangeRate { get; set; }

        public bool? IsBase { get; set; }

		public virtual ICollection<Transaction> Transactions { get; set; }
	}
}
