using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class RefundWorker : FactoryFloor<Refund>
    {
        public RefundWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override Refund GetById(Guid recordID)
        {
            return DataContext.Refund.FirstOrDefault(c => c.RefundId == recordID);
        }


        

        public override int UpdateCreate(Refund updateRecord)
        {
            if (Exists(updateRecord.RefundId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.Refund.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.Refund.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            Refund existingRecord = GetById(guid);
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
            return DataContext.Refund.Any(x => x.RefundId == guid);
        }
    }
}
