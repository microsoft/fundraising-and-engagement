using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundraisingandEngagement.DataverseSynchronization;
using Microsoft.Xrm.Sdk;

namespace DataverseSynchronizationTests
{
    class InMemoryDataVersionTokenRepository : IDataVersionTokenRepository
    {
        private readonly Dictionary<string, string> tokens = new Dictionary<string, string>();

        public void Put(string entityLogicalName, string dataVersionToken)
        {
            tokens[entityLogicalName] = dataVersionToken;
        }

        public string Get(string entityLogicalName)
        {
            if (tokens.ContainsKey(entityLogicalName))
            {
                return tokens[entityLogicalName];
            }
            return null;
        }
    }
}