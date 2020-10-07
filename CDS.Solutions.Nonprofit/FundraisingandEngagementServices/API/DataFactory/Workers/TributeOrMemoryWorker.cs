using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class TributeOrMemoryWorker : FactoryFloor<TributeOrMemory>
    {
        public TributeOrMemoryWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override TributeOrMemory GetById(Guid tributeOrMemoryId)
        {
            return DataContext.TributeOrMemory.FirstOrDefault(t => t.TributeOrMemoryId == tributeOrMemoryId);
        }


        public override int UpdateCreate(TributeOrMemory updateRecord)
        {
            if (Exists(updateRecord.TributeOrMemoryId))
            {
                updateRecord.SyncDate = DateTime.Now;

                DataContext.TributeOrMemory.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.TributeOrMemory.Add(updateRecord);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            TributeOrMemory existingRecord = GetById(guid);
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
            return DataContext.TributeOrMemory.Any(x => x.TributeOrMemoryId == guid);
        }
    }
}
