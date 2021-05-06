﻿using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class MembershipOrderWorker : IFactoryFloor<MembershipOrder>
    {
        private PaymentContext DataContext;

        public MembershipOrderWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public MembershipOrder GetById(Guid recordID)
        {
            return DataContext.MembershipOrder.FirstOrDefault(c => c.MembershipOrderId == recordID);
        }


        public int UpdateCreate(MembershipOrder updateRecord)
        {
            if (Exists(updateRecord.MembershipOrderId))
            {
                updateRecord.SyncDate = DateTime.Now;

                DataContext.MembershipOrder.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.MembershipOrder.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            MembershipOrder existingRecord = GetById(guid);
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
            return DataContext.MembershipOrder.Any(x => x.MembershipOrderId == guid);
        }
    }
}