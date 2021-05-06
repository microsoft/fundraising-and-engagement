﻿using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_GiftAidDeclaration")]
    public partial class GiftAidDeclaration : PaymentEntity, IContactPaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_giftaiddeclarationid")]
        public Guid GiftAidDeclarationId { get; set; }

        [EntityReferenceMap("msnfp_CustomerId")]
        public Guid? CustomerId { get; set; }

        public int? CustomerIdType { get; set; }

        [EntityNameMap("msnfp_declarationdate")]
        public DateTime? DeclarationDate { get; set; }

        [EntityOptionSetMap("msnfp_declarationdelivered")]
        public int? DeclarationDelivered { get; set; }

        [EntityNameMap("msnfp_giftaiddeclarationhtml")]
        public string GiftAidDeclarationHtml { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        [EntityNameMap("msnfp_updated")]
        public DateTime? Updated { get; set; }
    }
}