﻿using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class RegistrationWorker : IFactoryFloor<Registration>
    {
        private PaymentContext DataContext;

        public RegistrationWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public Registration GetById(Guid registrationId)
        {
            return DataContext.Registration.FirstOrDefault(t => t.RegistrationId == registrationId);
        }


        public int UpdateCreate(Registration updateRecord)
        {
            if (Exists(updateRecord.RegistrationId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.Registration.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.Registration.Add(updateRecord);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            Registration existingRecord = GetById(guid);
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
            return DataContext.Registration.Any(x => x.RegistrationId == guid);
        }
    }
}