using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class PaymentWorker : FactoryFloor<Payment>
	{
		public PaymentWorker(PaymentContext context)
		{
			DataContext = context;
		}

		public override Payment GetById(Guid recordID)
		{
			return DataContext.Payments.FirstOrDefault(c => c.PaymentId == recordID);
		}


		public override int UpdateCreate(Payment updateRecord)
		{
			if (Exists(updateRecord.PaymentId))
			{

				updateRecord.SyncDate = DateTime.Now;

				DataContext.Payments.Update(updateRecord);
				return DataContext.SaveChanges();
			}
			else if (updateRecord != null)
			{
				updateRecord.CreatedOn = DateTime.Now;
				DataContext.Payments.Add(updateRecord);
				return DataContext.SaveChanges();
			}
			else
			{
				return 0;
			}
		}

		public override int Delete(Guid guid)
		{
			Payment existingRecord = GetById(guid);
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
			return DataContext.Payments.Any(x => x.PaymentId == guid);
		}
	}
}
