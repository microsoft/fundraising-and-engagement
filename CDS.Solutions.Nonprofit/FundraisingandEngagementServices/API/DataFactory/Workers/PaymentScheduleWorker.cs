using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class PaymentScheduleWorker : FactoryFloor<PaymentSchedule>
    {
        public PaymentScheduleWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override PaymentSchedule GetById(Guid recordID)
        {
            return DataContext.PaymentSchedule.FirstOrDefault(c => c.PaymentScheduleId == recordID);
        }


        public override int UpdateCreate(PaymentSchedule updateRecord)
        {
            if (Exists(updateRecord.PaymentScheduleId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.PaymentSchedule.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.PaymentSchedule.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            PaymentSchedule existingRecord = GetById(guid);
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
            return DataContext.PaymentSchedule.Any(x => x.PaymentScheduleId == guid);
        }
    }
}
