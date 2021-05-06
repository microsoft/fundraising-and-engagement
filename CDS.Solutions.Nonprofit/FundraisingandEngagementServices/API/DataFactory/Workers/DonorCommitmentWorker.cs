using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory
{
    public class DonorCommitmentWorker : IFactoryFloor<DonorCommitment>
    {
        private PaymentContext DataContext;

        public DonorCommitmentWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public DonorCommitment GetById(Guid id)
        {
            return DataContext.DonorCommitment.FirstOrDefault(dc => dc.DonorCommitmentId == id);
        }

        public int UpdateCreate(DonorCommitment updateRecord)
        {
            if (Exists(updateRecord.DonorCommitmentId))
            {
                updateRecord.SyncDate = DateTime.UtcNow;

                DataContext.DonorCommitment.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.UtcNow;
                DataContext.DonorCommitment.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            DonorCommitment existingRecord = GetById(guid);
            if (existingRecord != null)
            {
                existingRecord.Deleted = true;
                existingRecord.DeletedDate = DateTime.UtcNow;

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
            return DataContext.DonorCommitment.Any(x => x.DonorCommitmentId == guid);
        }
    }
}