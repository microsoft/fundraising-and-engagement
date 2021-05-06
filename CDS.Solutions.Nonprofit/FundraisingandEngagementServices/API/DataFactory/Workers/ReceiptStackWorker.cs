using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class ReceiptStackWorker : IFactoryFloor<ReceiptStack>
    {
        private PaymentContext DataContext;

        public ReceiptStackWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public ReceiptStack GetById(Guid recordID)
        {
            return DataContext.ReceiptStack.FirstOrDefault(c => c.ReceiptStackId == recordID);
        }



        public int UpdateCreate(ReceiptStack updateRecord)
        {
            if (Exists(updateRecord.ReceiptStackId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.ReceiptStack.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.ReceiptStack.Add(updateRecord);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            ReceiptStack existingRecord = GetById(guid);
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
            return DataContext.ReceiptStack.Any(x => x.ReceiptStackId == guid);
        }
    }
}