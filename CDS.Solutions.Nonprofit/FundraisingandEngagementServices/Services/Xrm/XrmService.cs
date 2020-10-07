using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using FundraisingandEngagement.Models;
using FundraisingandEngagement.Models.Options;
using Xrm.Crm.WebApi;
using Xrm.Crm.WebApi.Authorization;
using Xrm.Crm.WebApi.Exceptions;
using Xrm.Crm.WebApi.Models;
using Xrm.Crm.WebApi.Models.Requests;

namespace FundraisingandEngagement.Services.Xrm
{
	public sealed class XrmService : IXrmService
	{
		private readonly WebApi client;
		private bool entityDefinitionsLoaded = false;

		public XrmService(IOptions<CrmOptions> options)
		{
			var o = options.Value;
			var authentication = new ServerToServerAuthentication(o.CrmApplicationID, o.CrmApplicationKey, o.CrmOrganizationURL, o.CrmTenantId);
			this.client = new WebApi(authentication);
		}

		public async Task<Guid> CreateAsync(Entity crmEntity)
		{
			try
			{
				await LoadEntityDefinitions();
				return await this.client.CreateAsync(crmEntity).ConfigureAwait(false);
			}
			catch (WebApiException e)
			{
				throw new XrmException(e);
			}
		}

		public async Task UpdateAsync(Entity crmEntity)
		{
			try
			{
				await LoadEntityDefinitions();
				await this.client.UpdateAsync(crmEntity).ConfigureAwait(false);
			}
			catch (WebApiException e)
			{
				throw new XrmException(e);
			}
		}

		public async Task<Entity> GetAsync(string logicalName, Guid id, params string[] properties)
		{
			try
			{
				await LoadEntityDefinitions();
				return await this.client.RetrieveAsync(logicalName, id, properties).ConfigureAwait(false);
			}
			catch (WebApiException e)
			{
				throw new XrmException(e);
			}
		}

		public async Task<IReadOnlyList<Entity>> GetListAsync(string entityCollection, params string[] properties)
		{
			try
			{
				await LoadEntityDefinitions();
				var options = new RetrieveOptions { Select = properties };
				var response = await this.client.RetrieveMultipleAsync(entityCollection, options).ConfigureAwait(false);
				return response.Entities;
			}
			catch (WebApiException e)
			{
				throw new XrmException(e);
			}
		}

		public async Task<IReadOnlyList<Entity>> GetFilteredListAsync(string entityCollection, string filter, params string[] properties)
		{
			try
			{
				await LoadEntityDefinitions();
				var options = new RetrieveOptions { Select = properties, Filter = filter };
				var response = await this.client.RetrieveMultipleAsync(entityCollection, options).ConfigureAwait(false);
				return response.Entities;
			}
			catch (WebApiException e)
			{
				throw new XrmException(e);
			}
		}

		public async Task DisassociateAsync(Entity crmEntity, string navigationProperty)
		{
			try
			{
				await LoadEntityDefinitions();
				await this.client.DisassociateAsync(crmEntity, navigationProperty);
			}
			catch (WebApiException e)
			{
				throw new XrmException(e);
			}
		}

		private async Task LoadEntityDefinitions()
		{
			if (this.entityDefinitionsLoaded == false)
			{
				await this.client.WebApiMetadata.LoadEntityDefinitions().ConfigureAwait(false);
				this.entityDefinitionsLoaded = true;
			}
		}
	}
}
