using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class SponsorshipWorker : IFactoryFloor<Sponsorship>
    {
        private PaymentContext DataContext;

        public SponsorshipWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public Sponsorship GetById(Guid sponsorshipId)
        {
            return DataContext.Sponsorship.FirstOrDefault(t => t.SponsorshipId == sponsorshipId);
        }


        public int UpdateCreate(Sponsorship updateRecord)
        {
            if (Exists(updateRecord.SponsorshipId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.Sponsorship.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.Sponsorship.Add(updateRecord);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            Sponsorship existingRecord = GetById(guid);
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
            return DataContext.Sponsorship.Any(x => x.SponsorshipId == guid);
        }
    }
}