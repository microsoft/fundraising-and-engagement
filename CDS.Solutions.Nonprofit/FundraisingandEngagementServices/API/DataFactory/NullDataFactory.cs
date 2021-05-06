using FundraisingandEngagement.Data;
using FundraisingandEngagement.DataFactory.Workers;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory
{
    public class NullDataFactory : IDataFactory
    {
        public IFactoryFloor<T> GetDataFactory<T>() where T : PaymentEntity
        {
            return new NullFactoryFloor<T>();
        }
    }
}