using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class PreferenceCategoryWorker : FactoryFloor<PreferenceCategory>
    {
        public PreferenceCategoryWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override PreferenceCategory GetById(Guid preferenceCategoryId)
        {
            return DataContext.PreferenceCategory.FirstOrDefault(t => t.preferencecategoryid == preferenceCategoryId);
        }



        public override int UpdateCreate(PreferenceCategory preferenceCategoryRecord)
        {

            if (Exists(preferenceCategoryRecord.preferencecategoryid))
            {
                preferenceCategoryRecord.SyncDate = DateTime.Now;

                DataContext.PreferenceCategory.Update(preferenceCategoryRecord);
                return DataContext.SaveChanges();
            }
            else if (preferenceCategoryRecord != null)
            {
                preferenceCategoryRecord.CreatedOn = DateTime.Now;
                DataContext.PreferenceCategory.Add(preferenceCategoryRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            PreferenceCategory existingRecord = GetById(guid);
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
            return DataContext.PreferenceCategory.Any(x => x.preferencecategoryid == guid);
        }
    }
}
