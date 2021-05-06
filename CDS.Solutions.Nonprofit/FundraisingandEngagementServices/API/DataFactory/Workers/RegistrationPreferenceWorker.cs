using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class RegistrationPreferenceWorker : IFactoryFloor<RegistrationPreference>
    {
        private PaymentContext DataContext;

        public RegistrationPreferenceWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public RegistrationPreference GetById(Guid registrationPreferenceId)
        {
            return DataContext.RegistrationPreference.FirstOrDefault(t => t.RegistrationPreferenceId == registrationPreferenceId);
        }

        public int UpdateCreate(RegistrationPreference registrationPreferenceRecord)
        {

            if (Exists(registrationPreferenceRecord.RegistrationPreferenceId))
            {
                registrationPreferenceRecord.SyncDate = DateTime.Now;

                DataContext.RegistrationPreference.Update(registrationPreferenceRecord);
                return DataContext.SaveChanges();
            }
            else if (registrationPreferenceRecord != null)
            {
                registrationPreferenceRecord.CreatedOn = DateTime.Now;
                DataContext.RegistrationPreference.Add(registrationPreferenceRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            RegistrationPreference existingRecord = GetById(guid);
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
            return DataContext.RegistrationPreference.Any(x => x.RegistrationPreferenceId == guid);
        }
    }
}