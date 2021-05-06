using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class PaymentWorker : IFactoryFloor<Payment>
    {
        private PaymentContext DataContext;

        public PaymentWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public Payment GetById(Guid recordID)
        {
            return DataContext.Payment.FirstOrDefault(c => c.PaymentId == recordID);
        }


        public int UpdateCreate(Payment updateRecord)
        {
            if (Exists(updateRecord.PaymentId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.Payment.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.Payment.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            Payment existingRecord = GetById(guid);
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
            return DataContext.Payment.Any(x => x.PaymentId == guid);
        }
    }
}