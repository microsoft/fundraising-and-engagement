using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class PaymentMethodWorker : IFactoryFloor<PaymentMethod>
    {
        private PaymentContext DataContext;

        public PaymentMethodWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public PaymentMethod GetById(Guid recordID)
        {
            return DataContext.PaymentMethod.FirstOrDefault(c => c.PaymentMethodId == recordID);
        }


        public int UpdateCreate(PaymentMethod updateRecord)
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

        public int Delete(Guid guid)
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


        public bool Exists(Guid guid)
        {
            return DataContext.PaymentMethod.Any(x => x.PaymentMethodId == guid);
        }
    }
}