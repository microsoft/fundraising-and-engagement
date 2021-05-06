using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundraisingandEngagement.DataverseSynchronization;
using Microsoft.Xrm.Sdk;

namespace DataverseSynchronizationTests
{
    class InMemoryEntityRepository : IEntityRepository
    {
        private readonly Dictionary<EntityKey, Entity> _entities = new Dictionary<EntityKey, Entity>();

        public void Upsert(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                _entities[new EntityKey(entity)] = entity;
            }
        }

        public void Delete(string entityName, Guid id)
        {
            _entities.Remove(new EntityKey(entityName, id));
        }

        public void RetainOnly(string entityLogicalName, IEnumerable<Guid> entityIdsToRetain)
        {
            var keysToDelete = from entry in _entities
                               where entry.Key.Name == entityLogicalName && !entityIdsToRetain.Contains(entry.Key.Id)
                               select entry.Key;
            keysToDelete.ToList().ForEach(
                key => _entities.Remove(key)
            );
        }

        public IEnumerable<Entity> AllEntities
        {
            get => _entities.Values;
        }

        private struct EntityKey
        {
            public EntityKey(Entity entity)
            {
                Name = entity.LogicalName;
                Id = entity.Id;
            }

            public EntityKey(string name, Guid id)
            {
                Name = name;
                Id = id;
            }

            public Guid Id { get; }
            public string Name { get; }
        }
    }
}