using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class PreferenceWorker : FactoryFloor<Preference>
    {
        public PreferenceWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override Preference GetById(Guid preferenceId)
        {
            return DataContext.Preference.FirstOrDefault(t => t.preferenceid == preferenceId);
        }



        public override int UpdateCreate(Preference preferenceRecord)
        {

            if (Exists(preferenceRecord.preferenceid))
            {
                preferenceRecord.SyncDate = DateTime.Now;

                DataContext.Preference.Update(preferenceRecord);
                return DataContext.SaveChanges();
            }
            else if (preferenceRecord != null)
            {
                preferenceRecord.CreatedOn = DateTime.Now;
                DataContext.Preference.Add(preferenceRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            Preference existingRecord = GetById(guid);
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
            return DataContext.Preference.Any(x => x.preferenceid == guid);
        }
    }
}
