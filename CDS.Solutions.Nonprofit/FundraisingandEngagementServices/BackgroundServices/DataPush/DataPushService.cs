using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Services.Xrm;

namespace FundraisingandEngagement.BackgroundServices.DataPush
{
	public class DataPushService
	{
		private readonly IPaymentContext context;
		private readonly IXrmService xrmService;
		private readonly IList<Type> entityType;
		private readonly ILogger logger;

		public DataPushService(IPaymentContext context, IXrmService xrmService,	ILogger logger)
		{
			this.logger = logger;
			this.context = context;
			this.xrmService = xrmService;
			this.entityType = new List<Type>();
		}

		public async Task PushAsync<T>(CancellationToken cancellationToken, Expression<Func<T, bool>>? filter = default) where T : class, IPaymentEntity, new()
		{
			if (cancellationToken.IsCancellationRequested)
				return;

			var type = typeof(T);
			if (this.entityType.Contains(type))
			{
				this.logger.LogWarning($"{type.Name} entities already have been pushed to Dynamics.");
				return;
			}

			this.entityType.Add(type);

			IQueryable<T> query = from entity in this.context.Set<T>()
								  where entity.SyncDate == null
								  where entity.Deleted == false || entity.Deleted == null
								  orderby entity.CreatedOn
								  select entity;

			if (filter != null)
				query = query.Where(filter);

			await foreach (var item in query.Take(1000).AsAsyncEnumerable())
			{
				if (cancellationToken.IsCancellationRequested)
					break;

				var pk = this.context.GetPK(item);

				try
				{
					var entity = await this.xrmService.GetAsync<T>(pk);

					try
					{
						if (entity is ITransactionResultEntity tre)
						{
							if (String.Equals(tre.TransactionResult, "true", StringComparison.OrdinalIgnoreCase))
							{
								tre.TransactionResult = "Approved";
							}
						}

						this.logger.LogInformation($"Updating '{type.Name}' with PK {pk}");
						await this.xrmService.UpdateAsync(item);
						item.SyncDate = DateTime.Now;
					}
					catch (XrmException e)
					{
						LogSyncException(e, pk, type.Name, $"Could not update {type.Name} entity {pk}");
					}
				}
				catch (XrmException e) when (e.EntityDoesNotExists)
				{
					try
					{
						this.logger.LogInformation($"Creating '{type.Name}' with PK {pk}");
						await this.xrmService.CreateAsync(item);
						item.SyncDate = DateTime.Now;
					}
					catch (XrmException ex)
					{
						LogSyncException(ex, pk, type.Name, $"Could not create {type.Name} entity {pk}");
					}
				}
				catch (XrmException e)
				{
					LogSyncException(e, pk, type.Name, $"Could not retrive {type.Name} entity {pk}");
				}
			}

			await this.context.SaveChangesAsync();
		}

		private void LogSyncException(Exception e, Guid pk, string type, string message)
		{
			this.logger.LogError(e, message);

			var syncEx = new SyncLog()
			{
				PaymentEntityPK = pk,
				EntityType = type,
				ExceptionMessage = message,
				StackTrace = e.ToString(),
				CreatedOn = DateTime.Now
			};

			if (type == nameof(Transaction))
				syncEx.TransactionId = pk;

			this.context.Add(syncEx);
		}
	}
}
