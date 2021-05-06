using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundraisingandEngagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundraisingandEngagement.DataFactory
{
    public interface IFactoryFloor<in T>
    {
        public int UpdateCreate(T entity);
        public int Delete(Guid guid);
        public bool Exists(Guid guid);
    }
}