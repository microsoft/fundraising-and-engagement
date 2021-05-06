using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class ReceiptLogWorker : IFactoryFloor<ReceiptLog>
    {
        private PaymentContext DataContext;

        public ReceiptLogWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public ReceiptLog GetById(Guid receiptLogId)
        {
            return DataContext.ReceiptLog.FirstOrDefault(t => t.ReceiptLogId == receiptLogId);
        }


        public int UpdateCreate(ReceiptLog updateRecord)
        {
            if (Exists(updateRecord.ReceiptLogId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.ReceiptLog.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.ReceiptLog.Add(updateRecord);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            ReceiptLog existingRecord = GetById(guid);
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
            return DataContext.ReceiptLog.Any(x => x.ReceiptLogId == guid);
        }
    }
}