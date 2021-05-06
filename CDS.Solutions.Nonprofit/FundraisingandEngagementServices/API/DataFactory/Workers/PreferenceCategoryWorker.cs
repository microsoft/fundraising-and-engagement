using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class PreferenceCategoryWorker : IFactoryFloor<PreferenceCategory>
    {
        private PaymentContext DataContext;

        public PreferenceCategoryWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public PreferenceCategory GetById(Guid preferenceCategoryId)
        {
            return DataContext.PreferenceCategory.FirstOrDefault(t => t.preferencecategoryid == preferenceCategoryId);
        }



        public int UpdateCreate(PreferenceCategory preferenceCategoryRecord)
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

        public int Delete(Guid guid)
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

        public bool Exists(Guid guid)
        {
            return DataContext.PreferenceCategory.Any(x => x.preferencecategoryid == guid);
        }
    }
}