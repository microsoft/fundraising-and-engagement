using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class SponsorshipWorker : FactoryFloor<Sponsorship>
    {
        public SponsorshipWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override Sponsorship GetById(Guid sponsorshipId)
        {
            return DataContext.Sponsorship.FirstOrDefault(t => t.SponsorshipId == sponsorshipId);
        }


        public override int UpdateCreate(Sponsorship updateRecord)
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

        public override int Delete(Guid guid)
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

        public override bool Exists(Guid guid)
        {
            return DataContext.Sponsorship.Any(x => x.SponsorshipId == guid);
        }
    }
}
