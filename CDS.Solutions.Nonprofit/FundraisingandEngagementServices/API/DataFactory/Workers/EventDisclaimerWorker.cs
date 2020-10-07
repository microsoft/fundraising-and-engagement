using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class EventDisclaimerWorker: FactoryFloor<EventDisclaimer>
    {
        public EventDisclaimerWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override EventDisclaimer GetById(Guid eventDisclaimerId)
        {
            return DataContext.EventDisclaimer.FirstOrDefault(t => t.EventDisclaimerId == eventDisclaimerId);
        }


        public override int UpdateCreate(EventDisclaimer eventDisclaimer)
        {

            if (Exists(eventDisclaimer.EventDisclaimerId))
            {
                eventDisclaimer.SyncDate = DateTime.Now;
                DataContext.EventDisclaimer.Update(eventDisclaimer);
                return DataContext.SaveChanges();
            }
            else if (eventDisclaimer != null)
            {
                eventDisclaimer.CreatedOn = DateTime.Now;
                DataContext.EventDisclaimer.Add(eventDisclaimer);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            EventDisclaimer existingRecord = GetById(guid);
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
            return DataContext.EventDisclaimer.Any(x => x.EventDisclaimerId == guid);
        }

    }
}
