using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.DataverseSynchronization;
using FundraisingandEngagement.Models.Entities;

namespace Data.DataSynchronization
{
    public class DbDataVersionTokenRepository : IDataVersionTokenRepository
    {
        private readonly PaymentContext _dbContext;

        public DbDataVersionTokenRepository(PaymentContext paymentContext)
        {
            this._dbContext = paymentContext;
        }

        public void Put(string entityLogicalName, string dataVersionToken)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var dbToken = _dbContext.DataverseSyncToken.FirstOrDefault(it => it.EntityLogicalName == entityLogicalName);
                if (dbToken != null)
                {
                    dbToken.TokenValue = dataVersionToken;
                    dbToken.UpdatedOn = DateTime.Now;
                    _dbContext.DataverseSyncToken.Update(dbToken);
                }
                else
                {
                    _dbContext.DataverseSyncToken.Add(new DataverseSyncToken { EntityLogicalName = entityLogicalName, TokenValue = dataVersionToken, UpdatedOn = DateTime.Now });
                }
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }

        public string Get(string entityLogicalName)
        {
            return _dbContext.DataverseSyncToken.FirstOrDefault(token => token.EntityLogicalName == entityLogicalName)?.TokenValue;
        }

    }
}