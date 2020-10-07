using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class EventTableWorker : FactoryFloor<EventTable>
    {
        public EventTableWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override EventTable GetById(Guid eventTableId)
        {
            return DataContext.EventTable.FirstOrDefault(t => t.EventTableId == eventTableId);
        }



        public override int UpdateCreate(EventTable eventTableRecord)
        {

            if (Exists(eventTableRecord.EventTableId))
            {
                eventTableRecord.SyncDate = DateTime.Now;

                DataContext.EventTable.Update(eventTableRecord);
                return DataContext.SaveChanges();
            }
            else if (eventTableRecord != null)
            {
                eventTableRecord.CreatedOn = DateTime.Now;
                DataContext.EventTable.Add(eventTableRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            EventTable existingRecord = GetById(guid);
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
            return DataContext.EventTable.Any(x => x.EventTableId == guid);
        }
    }
}
