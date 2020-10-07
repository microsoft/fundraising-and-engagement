using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrm.Crm.WebApi.Models;

namespace FundraisingandEngagement.Services.Xrm
{
	public interface IXrmService
	{
		Task<Guid> CreateAsync(Entity crmEntity);

		Task UpdateAsync(Entity crmEntity);

		Task DisassociateAsync(Entity crmEntity, string navigationProperty);

		Task<Entity> GetAsync(string logicalName, Guid id, params string[] properties);

		Task<IReadOnlyList<Entity>> GetListAsync(string entityCollection, params string[] properties);

		Task<IReadOnlyList<Entity>> GetFilteredListAsync(string entityCollection, string filter, params string[] properties);
	}
}