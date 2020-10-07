using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class EventProductWorker : FactoryFloor<EventProduct>
    {
        public EventProductWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override EventProduct GetById(Guid eventProductId)
        {
            return DataContext.EventProduct.FirstOrDefault(t => t.EventProductId == eventProductId);
        }


        public override int UpdateCreate(EventProduct eventProduct)
        {

            if (Exists(eventProduct.EventProductId))
            {
                eventProduct.SyncDate = DateTime.Now;
                DataContext.EventProduct.Update(eventProduct);
                return DataContext.SaveChanges();
            }
            else if (eventProduct != null)
            {
                eventProduct.CreatedOn = DateTime.Now;
                DataContext.EventProduct.Add(eventProduct);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            EventProduct existingRecord = GetById(guid);
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
            return DataContext.EventProduct.Any(x => x.EventProductId == guid);
        }
    }
}
