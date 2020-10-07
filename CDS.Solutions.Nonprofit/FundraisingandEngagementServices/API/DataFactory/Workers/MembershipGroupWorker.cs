using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class MembershipGroupWorker : FactoryFloor<MembershipGroup>
    {
        public MembershipGroupWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override MembershipGroup GetById(Guid recordID)
        {
            return DataContext.MembershipGroup.FirstOrDefault(c => c.MembershipGroupId == recordID);
        }



        public override int UpdateCreate(MembershipGroup updateRecord)
        {

            if (Exists(updateRecord.MembershipGroupId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.MembershipGroup.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.MembershipGroup.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            MembershipGroup existingRecord = GetById(guid);
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
            return DataContext.MembershipGroup.Any(x => x.MembershipGroupId == guid);
        }

    }
}
