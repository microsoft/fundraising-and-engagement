using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class TicketWorker : IFactoryFloor<Ticket>
    {
        private PaymentContext DataContext;

        public TicketWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public Ticket GetById(Guid ticketId)
        {
            return DataContext.Ticket.FirstOrDefault(t => t.TicketId == ticketId);
        }


        public int UpdateCreate(Ticket updateRecord)
        {
            if (Exists(updateRecord.TicketId))
            {
                updateRecord.SyncDate = DateTime.Now;

                DataContext.Ticket.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {

                updateRecord.CreatedOn = DateTime.Now;
                DataContext.Ticket.Add(updateRecord);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            Ticket existingRecord = GetById(guid);
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
            return DataContext.Ticket.Any(x => x.TicketId == guid);
        }

    }
}