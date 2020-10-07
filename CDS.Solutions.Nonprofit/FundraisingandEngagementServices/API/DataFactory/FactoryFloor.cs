using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory
{
	public abstract class FactoryFloor<T> : FactoryManager where T : PaymentEntity
    {
        public virtual async Task<IReadOnlyCollection<T>> Get(int take = 1000)
        {
            return await this.DataContext.Set<T>().Take(take).ToListAsync();
        }

        public abstract T GetById(Guid id);
        public abstract int UpdateCreate(T entity);
        public abstract int Delete(Guid guid);
        public abstract bool Exists(Guid guid);
    }
}
