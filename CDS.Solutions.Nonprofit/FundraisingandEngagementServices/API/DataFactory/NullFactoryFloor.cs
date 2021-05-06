using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundraisingandEngagement.DataFactory;

namespace FundraisingandEngagement.DataFactory
{
    public class NullFactoryFloor<T> : IFactoryFloor<T>

    {
        public int UpdateCreate(T entity)
        {
            return 1;
        }

        public int Delete(Guid guid)
        {
            return 0;
        }

        public bool Exists(Guid guid)
        {
            return false;
        }
    }
}