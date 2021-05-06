using System;
using System.Collections.Generic;
using System.Text;

namespace FundraisingandEngagement.Data.DataSynchronization
{
    public interface IDbEntitySynchronizer
    {
        void SynchronizeEntitiesToDbTransitively(List<Type> entitiesToSynchronize);
    }
}