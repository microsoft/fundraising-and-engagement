using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class MembershipCategoryWorker : FactoryFloor<MembershipCategory>
    {
        public MembershipCategoryWorker(PaymentContext context)
        {
            DataContext = context;
        }


        public override MembershipCategory GetById(Guid recordID)
        {
            return DataContext.MembershipCategory.FirstOrDefault(c => c.MembershipCategoryId == recordID);
        }


        public override int UpdateCreate(MembershipCategory updateRecord)
        {

            if (Exists(updateRecord.MembershipCategoryId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.MembershipCategory.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.MembershipCategory.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            MembershipCategory existingRecord = GetById(guid);
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
            return DataContext.MembershipCategory.Any(x => x.MembershipCategoryId == guid);
        }
    }
}
