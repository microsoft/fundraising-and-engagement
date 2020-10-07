using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class DesignationWorker : FactoryFloor<Designation>
    {
        public DesignationWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override Designation GetById(Guid DesignationId)
        {
            return DataContext.Designation.FirstOrDefault(t => t.DesignationId == DesignationId);
        }

        public string UpdateCreateReturnGuid(Designation updateRecord)
        {
            if (Exists(updateRecord.DesignationId))
            {
                updateRecord.SyncDate = DateTime.Now;

                DataContext.Designation.Update(updateRecord);
                DataContext.SaveChanges();

                return updateRecord.DesignationId.ToString();
            }
            else if (updateRecord != null)
            {

                DataContext.Designation.Add(updateRecord);

                DataContext.SaveChanges();

                return updateRecord.DesignationId.ToString();
            }
            else
            {
                return "Error";
            }
        }

        public override int UpdateCreate(Designation updateRecord)
        {
            if (Exists(updateRecord.DesignationId))
            {
                updateRecord.SyncDate = DateTime.Now;

                DataContext.Designation.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.Designation.Add(updateRecord);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            Designation existingRecord = GetById(guid);
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
            return DataContext.Designation.Any(x => x.DesignationId == guid);
        }

    }
}
