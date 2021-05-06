using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class EventSponsorWorker : IFactoryFloor<EventSponsor>
    {
        private PaymentContext DataContext;

        public EventSponsorWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public EventSponsor GetById(Guid eventSponsorId)
        {
            return DataContext.EventSponsor.FirstOrDefault(t => t.EventSponsorId == eventSponsorId);
        }


        public int UpdateCreate(EventSponsor eventSponsor)
        {
            if (Exists(eventSponsor.EventSponsorId))
            {
                DataContext.EventSponsor.Update(eventSponsor);
                return DataContext.SaveChanges();
            }
            else if (eventSponsor != null)
            {
                eventSponsor.CreatedOn = DateTime.Now;
                DataContext.EventSponsor.Add(eventSponsor);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            EventSponsor existingRecord = GetById(guid);
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
            return DataContext.EventSponsor.Any(x => x.EventSponsorId == guid);
        }
    }
}