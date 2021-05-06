using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class ContactWorker : IFactoryFloor<Contact>
    {
        private PaymentContext DataContext;

        public ContactWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public Contact GetById(Guid recordID)
        {
            return DataContext.Contact.FirstOrDefault(c => c.ContactId == recordID);
        }


        public int UpdateCreate(Contact updateRecord)
        {
            if (Exists(updateRecord.ContactId))
            {
                updateRecord.SyncDate = DateTime.Now;

                DataContext.Contact.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.Now;
                DataContext.Contact.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }


        public int Delete(Guid guid)
        {
            Contact existingRecord = GetById(guid);
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
            return DataContext.Contact.Any(x => x.ContactId == guid);
        }

    }
}