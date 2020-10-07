using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class EventPreferenceWorker : FactoryFloor<EventPreference>
    {
        public EventPreferenceWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override EventPreference GetById(Guid eventPreferenceId)
        {
            return DataContext.EventPreference.FirstOrDefault(t => t.EventPreferenceId == eventPreferenceId);
        }



        public override int UpdateCreate(EventPreference eventPreferenceRecord)
        {

            if (Exists(eventPreferenceRecord.EventPreferenceId))
            {
                eventPreferenceRecord.SyncDate = DateTime.Now;

                DataContext.EventPreference.Update(eventPreferenceRecord);
                return DataContext.SaveChanges();
            }
            else if (eventPreferenceRecord != null)
            {
                eventPreferenceRecord.CreatedOn = DateTime.Now;
                DataContext.EventPreference.Add(eventPreferenceRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            EventPreference existingRecord = GetById(guid);
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
            return DataContext.EventPreference.Any(x => x.EventPreferenceId == guid);
        }
    }
}
