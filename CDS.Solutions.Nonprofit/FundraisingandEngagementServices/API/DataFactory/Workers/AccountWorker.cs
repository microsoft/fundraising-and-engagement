using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class AccountWorker : FactoryFloor<Account>
    {
        public AccountWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override Account GetById(Guid recordID)
        {
            return DataContext.Account.FirstOrDefault(c => c.AccountId == recordID);
        }


        public override int UpdateCreate(Account updateRecord)
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

        public override int Delete(Guid guid)
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

        public override bool Exists(Guid guid)
        {
            return DataContext.Account.Any(x => x.AccountId == guid);
        }

    }
}
