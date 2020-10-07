using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class PaymentMethodWorker : FactoryFloor<PaymentMethod>
    {
        public PaymentMethodWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override PaymentMethod GetById(Guid recordID)
        {
            return DataContext.PaymentMethod.FirstOrDefault(c => c.PaymentMethodId == recordID);
        }


        public override int UpdateCreate(PaymentMethod updateRecord)
        {
            if (Exists(updateRecord.PaymentMethodId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.PaymentMethod.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.PaymentMethod.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            PaymentMethod existingRecord = GetById(guid);
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
            return DataContext.PaymentMethod.Any(x => x.PaymentMethodId == guid);
        }
    }
}
