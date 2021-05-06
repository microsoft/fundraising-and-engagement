using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory
{
    public class TransactionWorker : IFactoryFloor<Transaction>
    {
        private PaymentContext DataContext;

        public TransactionWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public Transaction GetById(Guid transactionId)
        {
            return DataContext.Transaction.FirstOrDefault(t => t.TransactionId == transactionId);
        }

        public int UpdateCreate(Transaction updateRecord)
        {
            if (Exists(updateRecord.TransactionId))
            {
                updateRecord.SyncDate = DateTime.UtcNow;

                DataContext.Transaction.Update(updateRecord);
                return DataContext.SaveChanges();
            }

            if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.UtcNow;
                this.DataContext.Transaction.Add(updateRecord);
                return this.DataContext.SaveChanges();
            }

            return 0;
        }

        public int Delete(Guid guid)
        {
            Transaction existingRecord = GetById(guid);
            if (existingRecord != null)
            {
                existingRecord.Deleted = true;
                existingRecord.DeletedDate = DateTime.UtcNow;

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
            return DataContext.Transaction.Any(x => x.TransactionId == guid);
        }
    }
}