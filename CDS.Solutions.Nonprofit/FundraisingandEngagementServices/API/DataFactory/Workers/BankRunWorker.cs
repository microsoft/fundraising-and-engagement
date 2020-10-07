using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class BankRunWorker : FactoryFloor<BankRun>
	{
		public BankRunWorker(PaymentContext context)
		{
			DataContext = context;
		}

		public override BankRun GetById(Guid BankRunId)
		{
			return DataContext.BankRun.FirstOrDefault(t => t.BankRunId == BankRunId);
		}

		public string UpdateCreateReturnGuid(BankRun updateRecord)
		{
			if (Exists(updateRecord.BankRunId))
			{
				updateRecord.SyncDate = DateTime.Now;

				DataContext.BankRun.Update(updateRecord);
				DataContext.SaveChanges();

				return updateRecord.BankRunId.ToString();
			}
			else if (updateRecord != null)
			{

				DataContext.BankRun.Add(updateRecord);

				DataContext.SaveChanges();

				return updateRecord.BankRunId.ToString();
			}
			else
			{
				return "Error";
			}
		}

		public override int UpdateCreate(BankRun updateRecord)
		{
			if (Exists(updateRecord.BankRunId))
			{
				updateRecord.SyncDate = DateTime.Now;

				DataContext.BankRun.Update(updateRecord);
				return DataContext.SaveChanges();
			}
			else if (updateRecord != null)
			{
				updateRecord.CreatedOn = DateTime.Now;
				DataContext.BankRun.Add(updateRecord);

				return DataContext.SaveChanges();
			}
			else
			{
				return 0;
			}
		}

		public override int Delete(Guid guid)
		{
			BankRun existingRecord = GetById(guid);
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
			return DataContext.BankRun.Any(x => x.BankRunId == guid);
		}

	}
}
