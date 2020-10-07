using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class ReceiptWorker : FactoryFloor<Receipt>
    {
        public ReceiptWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override Receipt GetById(Guid recordID)
        {
            return DataContext.Receipt.FirstOrDefault(c => c.ReceiptId == recordID);
        }


        public override int UpdateCreate(Receipt updateRecord)
        {
            if (Exists(updateRecord.ReceiptId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.Receipt.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.Receipt.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            Receipt existingRecord = GetById(guid);
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
            return DataContext.Receipt.Any(x => x.ReceiptId == guid);
        }
    }
}
