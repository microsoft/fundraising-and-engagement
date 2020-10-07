using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
	public class ProductWorker : FactoryFloor<Product>
    {
        public ProductWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override Product GetById(Guid productId)
        {
            return DataContext.Product.FirstOrDefault(t => t.ProductId == productId);
        }

        

        public override int UpdateCreate(Product updateRecord)
        {
            if (Exists(updateRecord.ProductId))
            {

                updateRecord.SyncDate = DateTime.Now;

                DataContext.Product.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.Product.Add(updateRecord);

                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            Product existingRecord = GetById(guid);
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
            return DataContext.Product.Any(x => x.ProductId == guid);
        }
    }
}
