using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class EventDonationWorker : FactoryFloor<EventDonation>
    {
        public EventDonationWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override EventDonation GetById(Guid eventDonationId)
        {
            return DataContext.EventDonation.FirstOrDefault(t => t.EventDonationId == eventDonationId);
        }


        public override int UpdateCreate(EventDonation eventDonation)
        {
            if (Exists(eventDonation.EventDonationId))
            {
                eventDonation.SyncDate = DateTime.Now;
                DataContext.EventDonation.Update(eventDonation);
                return DataContext.SaveChanges();
            }
            else if (eventDonation != null)
            {
                eventDonation.CreatedOn = DateTime.Now;
                DataContext.EventDonation.Add(eventDonation);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            EventDonation existingRecord = GetById(guid);
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
            return DataContext.EventDonation.Any(c => c.EventDonationId == guid);
        }
    }
}
