using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class ReceiptLogWorker : FactoryFloor<ReceiptLog>
    {
        public ReceiptLogWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override ReceiptLog GetById(Guid receiptLogId)
        {
            return DataContext.ReceiptLog.FirstOrDefault(t => t.ReceiptLogId == receiptLogId);
        }


        public override int UpdateCreate(ReceiptLog updateRecord)
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

        public override int Delete(Guid guid)
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

        public override bool Exists(Guid guid)
        {
            return DataContext.ReceiptLog.Any(x => x.ReceiptLogId == guid);
        }
    }
}
