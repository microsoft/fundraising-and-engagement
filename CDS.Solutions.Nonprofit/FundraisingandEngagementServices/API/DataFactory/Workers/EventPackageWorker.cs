using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class EventPackageWorker : FactoryFloor<EventPackage>
    {
        public EventPackageWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override EventPackage GetById(Guid eventPackageId)
        {
            return DataContext.EventPackage.FirstOrDefault(t => t.EventPackageId == eventPackageId);
        }

        public override int UpdateCreate(EventPackage eventPackage)
        {

            if (Exists(eventPackage.EventPackageId))
            {
                eventPackage.SyncDate = DateTime.Now;

                DataContext.EventPackage.Update(eventPackage);
                return DataContext.SaveChanges();
            }
            else if (eventPackage != null)
            {
                eventPackage.CreatedOn = DateTime.Now;
                DataContext.EventPackage.Add(eventPackage);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            EventPackage existingRecord = GetById(guid);
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
            return DataContext.EventPackage.Any(x => x.EventPackageId == guid);
        }
    }
}
