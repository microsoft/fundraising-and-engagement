using System;
using System.Collections.Generic;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
    public class DataverseSyncToken
    {
        public string EntityLogicalName { get; set; }
        public string TokenValue { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}