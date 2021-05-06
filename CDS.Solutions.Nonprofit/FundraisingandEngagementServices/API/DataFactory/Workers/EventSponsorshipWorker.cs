﻿using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class EventSponsorshipWorker : IFactoryFloor<EventSponsorship>
    {
        private PaymentContext DataContext;

        public EventSponsorshipWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public EventSponsorship GetById(Guid eventSponsorshipId)
        {
            return DataContext.EventSponsorship.FirstOrDefault(t => t.EventSponsorshipId == eventSponsorshipId);
        }



        public int UpdateCreate(EventSponsorship eventSponsorship)
        {

            if (Exists(eventSponsorship.EventSponsorshipId))
            {
                eventSponsorship.SyncDate = DateTime.Now;

                DataContext.EventSponsorship.Update(eventSponsorship);
                return DataContext.SaveChanges();
            }
            else if (eventSponsorship != null)
            {
                eventSponsorship.CreatedOn = DateTime.Now;
                DataContext.EventSponsorship.Add(eventSponsorship);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            EventSponsorship existingRecord = GetById(guid);
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
            return DataContext.EventSponsorship.Any(x => x.EventSponsorshipId == guid);
        }
    }
}