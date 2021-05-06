using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class TransactionCurrencyWorker : IFactoryFloor<TransactionCurrency>
    {
        private PaymentContext DataContext;

        public TransactionCurrencyWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public TransactionCurrency GetById(Guid recordID)
        {
            return DataContext.TransactionCurrency.FirstOrDefault(c => c.TransactionCurrencyId == recordID);
        }


        public int UpdateCreate(TransactionCurrency updateRecord)
        {
            if (Exists(updateRecord.TransactionCurrencyId))
            {
                updateRecord.SyncDate = DateTime.Now;

                DataContext.TransactionCurrency.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.TransactionCurrency.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            TransactionCurrency existingRecord = GetById(guid);
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
            return DataContext.TransactionCurrency.Any(x => x.TransactionCurrencyId == guid);
        }
    }
}