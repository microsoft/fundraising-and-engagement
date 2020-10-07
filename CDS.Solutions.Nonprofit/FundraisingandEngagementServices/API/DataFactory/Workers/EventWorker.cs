using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class EventWorker : FactoryFloor<Event>
    {
        public EventWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override Event GetById(Guid eventId)
        {
            return DataContext.Event.FirstOrDefault(t => t.EventId == eventId);
        }



        public override int UpdateCreate(Event eventRecord)
        {

            if (Exists(eventRecord.EventId))
            {
                eventRecord.SyncDate = DateTime.Now;

                DataContext.Event.Update(eventRecord);
                return DataContext.SaveChanges();
            }
            else if (eventRecord != null)
            {
                eventRecord.CreatedOn = DateTime.Now;
                DataContext.Event.Add(eventRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            Event existingRecord = GetById(guid);
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
            return DataContext.Event.Any(x => x.EventId == guid);
        }
    }
}
