// the code has a lot of warning messages, possble possible issues
#pragma warning disable CS8629 // Nullable value type may be null.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using FundraisingandEngagement.Services.Xrm;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;


namespace FundraisingandEngagement
{
	/// <summary>
	/// Performance metrics for Campaign / Appeal / Package
	/// </summary>
	public class PerformanceCalculation
	{
		private readonly PaymentContext dataContext;
		private readonly ILogger logger;
		private readonly IXrmService xrmService;


		public PerformanceCalculation(PaymentContext dataContext, IXrmService xrmService, ILogger logger)
		{
			this.logger = logger;
			this.dataContext = dataContext;
			this.xrmService = xrmService;
		}

		public async Task PerformanceMetricsStart(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
				return;

			await GetMetrics(cancellationToken);
		}

		/// <summary>
		/// Generate metrics for appeals / campaigns / packages from event packages
		/// </summary>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Task</returns>
		private async Task GetMetrics(CancellationToken cancellationToken)
		{
			var baseQuery = dataContext.EventPackage.Where(w => ((w.Deleted.HasValue && !w.Deleted.Value) || w.Deleted == null) && w.StatusCode == StatusCode.Completed).Join(dataContext.Event, ep => ep.EventId, e => e.EventId, (ep, e) => new { EventPackage = ep, AppealId = e.AppealId, CampaignId = e.CampaignId, PackageId = e.PackageId }).AsQueryable();
			// donor commitments are tracked outside of event package
			var donorCommitmentQuery = dataContext.DonorCommitment.Where(w => ((w.Deleted.HasValue && !w.Deleted.Value) || w.Deleted == null) && w.StatusCode == StatusCode.Completed);
			// transactions that are outside event package
			var transactionQuery = dataContext.Transaction.Where(w => ((w.Deleted.HasValue && !w.Deleted.Value) || w.Deleted == null) && w.StatusCode == StatusCode.Completed && !w.EventPackageId.HasValue);

			// appeals
			var appealsEventPackages = baseQuery.Where(w => w.AppealId.HasValue)
				.GroupBy(g => g.AppealId.Value)
				.Select(s => new Appeal
				{
					AppealId = s.Key,
					DonationAmount = s.Sum(su => su.EventPackage.ValDonations),
					DonationCount = s.Sum(su => su.EventPackage.SumDonations),
					ProductAmount = s.Sum(su => su.EventPackage.ValProducts),
					ProductCount = s.Sum(su => su.EventPackage.SumProducts),
					RegistrationAmount = s.Sum(su => su.EventPackage.ValTickets),
					RegistrationCount = s.Sum(su => su.EventPackage.SumTickets),
					SponsorshipAmount = s.Sum(su => su.EventPackage.ValSponsorships),
					SponsorshipCount = s.Sum(su => su.EventPackage.SumSponsorships),
					TotalAmount = s.Sum(su => su.EventPackage.ValDonations) + s.Sum(su => su.EventPackage.ValProducts) + s.Sum(su => su.EventPackage.ValTickets) + s.Sum(su => su.EventPackage.ValSponsorships),
				});


			var appealsDonorCommitments = donorCommitmentQuery.Where(w => w.AppealId.HasValue)
															.GroupBy(g => g.AppealId.Value)
															.Select(s => new Appeal
															{
																AppealId = s.Key,
																DonorCommitmentAmount = s.Sum(su => su.TotalAmount),
																DonorCommitmentCount = s.Count(),
																TotalAmount = s.Sum(su => su.TotalAmount)
															});

			var appealsTransactions = transactionQuery.Where(w => w.AppealId.HasValue)
															.GroupBy(g => g.AppealId.Value)
															.Select(s => new Appeal
															{
																AppealId = s.Key,
																DonationAmount = s.Sum(su => su.Amount),
																DonationCount = s.Count(),
																TotalAmount = s.Sum(su => su.Amount),
															});

			var appeals = appealsEventPackages.AsEnumerable()
						.Concat(appealsDonorCommitments.AsEnumerable())
						.Concat(appealsTransactions)
						.GroupBy(g => g.AppealId).Select(s => new Appeal
						{
							AppealId = s.Key,
							ProductAmount = s.Where(w => w.ProductAmount.HasValue).Sum(su => su.ProductAmount),
							ProductCount = s.Where(w => w.ProductCount.HasValue).Sum(su => su.ProductCount),
							RegistrationAmount = s.Where(w => w.RegistrationAmount.HasValue).Sum(su => su.RegistrationAmount),
							RegistrationCount = s.Where(w => w.RegistrationCount.HasValue).Sum(su => su.RegistrationCount),
							SponsorshipAmount = s.Where(w => w.SponsorshipAmount.HasValue).Sum(su => su.SponsorshipAmount),
							SponsorshipCount = s.Where(w => w.SponsorshipCount.HasValue).Sum(su => su.SponsorshipCount),
							DonationAmount = s.Where(w => w.DonationAmount.HasValue).Sum(su => su.DonationAmount),
							DonationCount = s.Where(w => w.DonationCount.HasValue).Sum(su => su.DonationCount),
							DonorCommitmentAmount = s.Where(w => w.DonorCommitmentAmount.HasValue).Sum(su => su.DonorCommitmentAmount),
							DonorCommitmentCount = s.Where(w => w.DonorCommitmentCount.HasValue).Sum(su => su.DonorCommitmentCount),
							TotalAmount = s.Where(w => w.TotalAmount.HasValue).Sum(su => su.TotalAmount),
						});

			// campaigns
			var campaignsEventPackages = baseQuery.Where(w => w.CampaignId.HasValue)
				.GroupBy(g => g.CampaignId.Value)
				.Select(s => new Campaign
				{
					CampaignId = s.Key,
					DonationAmount = s.Sum(su => su.EventPackage.ValDonations),
					DonationCount = s.Sum(su => su.EventPackage.SumDonations),
					ProductAmount = s.Sum(su => su.EventPackage.ValProducts),
					ProductCount = s.Sum(su => su.EventPackage.SumProducts),
					RegistrationAmount = s.Sum(su => su.EventPackage.ValTickets),
					RegistrationCount = s.Sum(su => su.EventPackage.SumTickets),
					SponsorshipAmount = s.Sum(su => su.EventPackage.ValSponsorships),
					SponsorshipCount = s.Sum(su => su.EventPackage.SumSponsorships),
					TotalAmount = s.Sum(su => su.EventPackage.ValDonations) + s.Sum(su => su.EventPackage.ValProducts) + s.Sum(su => su.EventPackage.ValTickets) + s.Sum(su => su.EventPackage.ValSponsorships),
				});

			var campaignsDonorCommitments = donorCommitmentQuery.Where(w => w.CampaignId.HasValue)
															.GroupBy(g => g.CampaignId.Value)
															.Select(s => new Campaign
															{
																CampaignId = s.Key,
																DonorCommitmentAmount = s.Sum(su => su.TotalAmount),
																DonorCommitmentCount = s.Count(),
																TotalAmount = s.Sum(su => su.TotalAmount)
															});

			var campaignsTransactions = transactionQuery.Where(w => w.OriginatingCampaignId.HasValue)
															.GroupBy(g => g.OriginatingCampaignId.Value)
															.Select(s => new Campaign
															{
																CampaignId = s.Key,
																DonationAmount = s.Sum(su => su.Amount),
																DonationCount = s.Count(),
																TotalAmount = s.Sum(su => su.Amount)
															});

			var campaigns = campaignsEventPackages.AsEnumerable()
						.Concat(campaignsDonorCommitments.AsEnumerable())
						.Concat(campaignsTransactions)
						.GroupBy(g => g.CampaignId).Select(s => new Campaign
						{
							CampaignId = s.Key,
							ProductAmount = s.Where(w => w.ProductAmount.HasValue).Sum(su => su.ProductAmount),
							ProductCount = s.Where(w => w.ProductCount.HasValue).Sum(su => su.ProductCount),
							RegistrationAmount = s.Where(w => w.RegistrationAmount.HasValue).Sum(su => su.RegistrationAmount),
							RegistrationCount = s.Where(w => w.RegistrationCount.HasValue).Sum(su => su.RegistrationCount),
							SponsorshipAmount = s.Where(w => w.SponsorshipAmount.HasValue).Sum(su => su.SponsorshipAmount),
							SponsorshipCount = s.Where(w => w.SponsorshipCount.HasValue).Sum(su => su.SponsorshipCount),
							DonationAmount = s.Where(w => w.DonationAmount.HasValue).Sum(su => su.DonationAmount),
							DonationCount = s.Where(w => w.DonationCount.HasValue).Sum(su => su.DonationCount),
							DonorCommitmentAmount = s.Where(w => w.DonorCommitmentAmount.HasValue).Sum(su => su.DonorCommitmentAmount),
							DonorCommitmentCount = s.Where(w => w.DonorCommitmentCount.HasValue).Sum(su => su.DonorCommitmentCount),
							TotalAmount = s.Where(w => w.TotalAmount.HasValue).Sum(su => su.TotalAmount),
						});

			// packages
			var packagesEventPackges = baseQuery.Where(w => w.PackageId.HasValue)
				.GroupBy(g => g.PackageId.Value)
				.Select(s => new Package
				{
					PackageId = s.Key,
					DonationAmount = s.Sum(su => su.EventPackage.ValDonations),
					DonationCount = s.Sum(su => su.EventPackage.SumDonations),
					ProductAmount = s.Sum(su => su.EventPackage.ValProducts),
					ProductCount = s.Sum(su => su.EventPackage.SumProducts),
					RegistrationAmount = s.Sum(su => su.EventPackage.ValTickets),
					RegistrationCount = s.Sum(su => su.EventPackage.SumTickets),
					SponsorshipAmount = s.Sum(su => su.EventPackage.ValSponsorships),
					SponsorshipCount = s.Sum(su => su.EventPackage.SumSponsorships),
					TotalAmount = s.Sum(su => su.EventPackage.ValDonations) + s.Sum(su => su.EventPackage.ValProducts) + s.Sum(su => su.EventPackage.ValTickets) + s.Sum(su => su.EventPackage.ValSponsorships),
				});

			var packagesDonorCommitments = donorCommitmentQuery.Where(w => w.PackageId.HasValue)
															.GroupBy(g => g.PackageId.Value)
															.Select(s => new Package
															{
																PackageId = s.Key,
																DonorCommitmentAmount = s.Sum(su => su.TotalAmount),
																DonorCommitmentCount = s.Count(),
																TotalAmount = s.Sum(su => su.TotalAmount)

															});

			var packagesTransactions = transactionQuery.Where(w => w.PackageId.HasValue)
															.GroupBy(g => g.PackageId.Value)
															.Select(s => new Package
															{
																PackageId = s.Key,
																DonationAmount = s.Sum(su => su.Amount),
																DonationCount = s.Count(),
																TotalAmount = s.Sum(su => su.Amount)
															});

			var packages = packagesEventPackges.AsEnumerable()
											.Concat(packagesDonorCommitments.AsEnumerable())
											.Concat(packagesTransactions.AsEnumerable())
											.GroupBy(g => g.PackageId).Select(s => new Package
											{
												PackageId = s.Key,
												ProductAmount = s.Where(w => w.ProductAmount.HasValue).Sum(su => su.ProductAmount),
												ProductCount = s.Where(w => w.ProductCount.HasValue).Sum(su => su.ProductCount),
												RegistrationAmount = s.Where(w => w.RegistrationAmount.HasValue).Sum(su => su.RegistrationAmount),
												RegistrationCount = s.Where(w => w.RegistrationCount.HasValue).Sum(su => su.RegistrationCount),
												SponsorshipAmount = s.Where(w => w.SponsorshipAmount.HasValue).Sum(su => su.SponsorshipAmount),
												SponsorshipCount = s.Where(w => w.SponsorshipCount.HasValue).Sum(su => su.SponsorshipCount),
												DonationAmount = s.Where(w => w.DonationAmount.HasValue).Sum(su => su.DonationAmount),
												DonationCount = s.Where(w => w.DonationCount.HasValue).Sum(su => su.DonationCount),
												DonorCommitmentAmount = s.Where(w => w.DonorCommitmentAmount.HasValue).Sum(su => su.DonorCommitmentAmount),
												DonorCommitmentCount = s.Where(w => w.DonorCommitmentCount.HasValue).Sum(su => su.DonorCommitmentCount),
												TotalAmount = s.Where(w => w.TotalAmount.HasValue).Sum(su => su.TotalAmount),
											});

			await UpdateCRM(packages, cancellationToken);
			await UpdateCRM(campaigns, cancellationToken);
			await UpdateCRM(appeals, cancellationToken);
		}

		/// <summary>
		/// Update data in CRM
		/// </summary>
		/// <typeparam name="T">Object of base type payment entity</typeparam>
		/// <param name="rows">Collection of T</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Task</returns>
		private async Task UpdateCRM<T>(IEnumerable<T> rows, CancellationToken cancellationToken) where T : PaymentEntity
		{
			this.logger.LogInformation("Starting update process..");

			foreach (var r in rows.Take(500))
			{
				if (cancellationToken.IsCancellationRequested || r == null)
					break;

				try
				{
					await this.xrmService.UpdateAsync(r);
				}
				catch (XrmException e)
				{
					if (e.EntityDoesNotExists)
						this.logger.LogWarning(e, $"Record not found in Dynamics.Could not update metrics");
					else
						this.logger.LogError(e, $"Could not update metrics");
				}
			}
		}
	}
}
