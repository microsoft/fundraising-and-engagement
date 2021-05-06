using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class BankRunScheduleWorker : IFactoryFloor<BankRunSchedule>
    {
        private PaymentContext DataContext;

        public BankRunScheduleWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public BankRunSchedule GetById(Guid BankRunScheduleId)
        {
            return DataContext.BankRunSchedule.FirstOrDefault(t => t.BankRunScheduleId == BankRunScheduleId);
        }

        public string UpdateCreateReturnGuid(BankRunSchedule updateRecord)
        {
            if (Exists(updateRecord.BankRunScheduleId))
            {
                updateRecord.SyncDate = DateTime.Now;

                DataContext.BankRunSchedule.Update(updateRecord);
                DataContext.SaveChanges();

                return updateRecord.BankRunScheduleId.ToString();
            }
            else if (updateRecord != null)
            {

                DataContext.BankRunSchedule.Add(updateRecord);

                DataContext.SaveChanges();

                return updateRecord.BankRunScheduleId.ToString();
            }
            else
            {
                return "Error";
            }
        }

        public int UpdateCreate(BankRunSchedule updateRecord)
        {
            if (Exists(updateRecord.BankRunScheduleId))
            {
                updateRecord.SyncDate = DateTime.Now;

                DataContext.BankRunSchedule.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.BankRunSchedule.Add(updateRecord);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int Delete(Guid guid)
        {
            BankRunSchedule existingRecord = GetById(guid);
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
            return DataContext.BankRunSchedule.Any(x => x.BankRunScheduleId == guid);
        }

    }
}