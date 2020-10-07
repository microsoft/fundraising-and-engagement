using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class PaymentProcessorWorker : FactoryFloor<PaymentProcessor>
    {
        public PaymentProcessorWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override PaymentProcessor GetById(Guid recordID)
        {
            return DataContext.PaymentProcessor.FirstOrDefault(c => c.PaymentProcessorId == recordID);
        }


        public override int UpdateCreate(PaymentProcessor updateRecord)
        {
            if (Exists(updateRecord.PaymentProcessorId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.PaymentProcessor.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.PaymentProcessor.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            PaymentProcessor existingRecord = GetById(guid);
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
            return DataContext.PaymentProcessor.Any(x => x.PaymentProcessorId == guid);
        }
    }
}