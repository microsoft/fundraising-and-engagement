using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class AccountWorker : IFactoryFloor<Account>
    {
        private PaymentContext DataContext;

        public AccountWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public Account GetById(Guid recordID)
        {
            return DataContext.Account.FirstOrDefault(c => c.AccountId == recordID);
        }


        public int UpdateCreate(Account updateRecord)
        {
            if (Exists(updateRecord.AccountId))
            {
                updateRecord.SyncDate = DateTime.Now;

                DataContext.Account.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                //updateRecord.CreatedOn = DateTime.Now;
                DataContext.Account.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            Account existingRecord = GetById(guid);
            if (existingRecord != null)
            {
                existingRecord.Deleted = true;
                existingRecord.DeletedDate = DateTime.Now;

                DataContext.Update(existingRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public bool Exists(Guid guid)
        {
            return DataContext.Account.Any(x => x.AccountId == guid);
        }

    }
}