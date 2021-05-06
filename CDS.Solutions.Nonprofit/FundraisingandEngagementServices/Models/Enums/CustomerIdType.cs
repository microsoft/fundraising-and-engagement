using System;
using System.Collections.Generic;
using System.Text;

namespace FundraisingandEngagement.Models.Enums
{
    public static class CustomerIdType
    {
        public const int Account = 1;
        public const int Contact = 2;

        public static int LogicalNameToType(string entityLogicalName)
        {
            switch (entityLogicalName)
            {
                case "contact": return Contact;
                case "account": return Account;
                default: throw new ArgumentException($"Unexpected entity for {nameof(CustomerIdType)}: {entityLogicalName}");
            }
        }
    }
}