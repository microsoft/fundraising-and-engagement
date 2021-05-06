using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory
{
    public interface IDataFactory
    {
        IFactoryFloor<T> GetDataFactory<T>() where T : PaymentEntity;
    }
}