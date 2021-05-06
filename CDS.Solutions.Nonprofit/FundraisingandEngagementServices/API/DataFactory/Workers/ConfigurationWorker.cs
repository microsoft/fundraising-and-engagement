using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory.Workers
{
    public class ConfigurationWorker : IFactoryFloor<Configuration>
    {
        private PaymentContext DataContext;

        public ConfigurationWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public Configuration GetById(Guid configurationId)
        {
            return DataContext.Configuration.FirstOrDefault(c => c.ConfigurationId == configurationId);
        }



        public int UpdateCreate(Configuration configuration)
        {
            if (Exists(configuration.ConfigurationId))
            {
                configuration.SyncDate = DateTime.Now;

                DataContext.Configuration.Update(configuration);
                return DataContext.SaveChanges();
            }
            else if (configuration != null)
            {
                configuration.CreatedOn = DateTime.Now;
                DataContext.Configuration.Add(configuration);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }


        public int Delete(Guid guid)
        {
            Configuration existingRecord = GetById(guid);
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
            return DataContext.Configuration.Any(c => c.ConfigurationId == guid);
        }
    }
}