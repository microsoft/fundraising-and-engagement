using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class ResponseWorker : FactoryFloor<Response>
    {
        public ResponseWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override Response GetById(Guid responseId)
        {
            return DataContext.Response.FirstOrDefault(t => t.ResponseId == responseId);
        }


        public override int UpdateCreate(Response updateRecord)
        {
            if (Exists(updateRecord.ResponseId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.Response.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.Response.Add(updateRecord);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            Response existingRecord = GetById(guid);
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
            return DataContext.Response.Any(x => x.ResponseId == guid);
        }
    }
}
