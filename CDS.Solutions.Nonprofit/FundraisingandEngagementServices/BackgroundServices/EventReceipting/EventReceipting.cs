using System;
using System.Collections.Generic;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement
{
	internal class EventReceipting
	{
		internal const string EventTicket = "msnfp_eventticket";
		internal const string EventProduct = "msnfp_eventproduct";
		internal const string EventSponsorship = "msnfp_eventsponsorship";

		internal static EventTicket GetEventTicketFromId(Guid entityId, IPaymentContext context)
		{
			var eventTicket = context.EventTicket.FirstOrDefault(et => et.EvenTicketId == entityId);
			if (eventTicket == null)
			{
				throw new Exception("Could not find Event Ticket with Id:" + entityId);
			}

			return eventTicket;
		}

		internal static EventProduct GetEventProductFromId(Guid entityId, IPaymentContext context)
		{
			var eventProduct = context.EventProduct.FirstOrDefault(ep => ep.EventProductId == entityId);
			if (eventProduct == null)
			{
				throw new Exception("Could not find Event Product  with Id:" + entityId);
			}

			return eventProduct;
		}

		internal static EventSponsorship GetEventSponsorshipFromId(Guid entityId, IPaymentContext context)
		{
			var eventSponsorship = context.EventSponsorship.FirstOrDefault(es => es.EventSponsorshipId == entityId);
			if (eventSponsorship == null)
			{
				throw new Exception("Could not find Event Sponsorship  with Id:" + entityId);
			}

			return eventSponsorship;
		}


		internal static void UpdateTicketsFromEventTicket(EventTicket eventTicket, PaymentContext context)
		{
			// get the list of all Tickets associated with this record
			var tickets = context.Ticket.Where(t => t.EventTicketId == eventTicket.EvenTicketId && t.StateCode == 0);

			// update their amount fields
			foreach (var curTicket in tickets)
			{
				curTicket.AmountNonreceiptable = eventTicket.AmountNonReceiptable ?? 0;
				curTicket.AmountReceipted = eventTicket.AmountReceipted ?? 0;
				curTicket.SyncDate = null;
				context.Update(curTicket);
			}

			context.SaveChanges();
		}


		internal static void UpdateProductsFromEventProduct(EventProduct eventProduct, PaymentContext context)
		{
			// get the list of all Products associated with this record
			var products = context.Product.Where(p => p.EventProductId == eventProduct.EventProductId && p.StateCode == 0).ToList();

			// update their amount fields
			foreach (var curProduct in products)
			{
				curProduct.AmountNonreceiptable = eventProduct.AmountNonReceiptable ?? 0;
				curProduct.AmountReceipted = eventProduct.AmountReceipted ?? 0;
				curProduct.SyncDate = null;
				context.Update(curProduct);
			}

			context.SaveChanges();
		}

		internal static void UpdateSponsorshipsFromEventSponsorship(EventSponsorship eventSponsorship,
			PaymentContext context)
		{
			// get the list of all Sponsorships associated with this record
			var sponsorships =
				context.Sponsorship.Where(s => s.EventSponsorshipId == eventSponsorship.EventSponsorshipId && s.StateCode == 0).ToList();

			// update their amount fields
			foreach (var curSponsorship in sponsorships)
			{
				curSponsorship.AmountNonreceiptable = eventSponsorship.AmountNonReceiptable ?? 0;
				curSponsorship.AmountReceipted = eventSponsorship.AmountReceipted ?? 0;
				curSponsorship.SyncDate = null;
				context.Update(curSponsorship);
			}

			context.SaveChanges();
		}


		internal static List<EventPackage> GetEventPackagesFromEventTicket(EventTicket eventTicket, IPaymentContext context)
		{
			var eventPackages = new List<EventPackage>();

			// get the list of all Tickets associated with this record
			var tickets = context.Ticket.Where(t => t.EventTicketId == eventTicket.EvenTicketId && t.StateCode == 0).ToList();

			// get Event Packages from the Tickets
			foreach (var curTicket in tickets)
			{
				if (curTicket.EventPackageId.HasValue)
				{
					var curEventPackages =
						context.EventPackage.Where(e => e.EventPackageId == curTicket.EventPackageId && e.StateCode == 0).Distinct().ToList();
					eventPackages.AddRange(curEventPackages);
				}
			}
			return eventPackages;
		}

		internal static List<EventPackage> GetEventPackagesFromEventProduct(EventProduct eventProduct, IPaymentContext context)
		{
			var eventPackages = new List<EventPackage>();

			// get the list of all Products associated with this record
			var products = context.Product.Where(t => t.EventProductId == eventProduct.EventProductId && t.StateCode == 0).ToList();

			// get Event Packages from the Products
			foreach (var curProduct in products)
			{
				if (curProduct.EventPackageId.HasValue)
				{
					var curEventPackages =
						context.EventPackage.Where(e => e.EventPackageId == curProduct.EventPackageId && e.StateCode == 0).Distinct().ToList();
					eventPackages.AddRange(curEventPackages);
				}
			}
			return eventPackages;
		}

		internal static List<EventPackage> GetEventPackagesFromEventSponsorship(EventSponsorship eventSponsorship, IPaymentContext context)
		{
			var eventPackages = new List<EventPackage>();

			// get the list of all Sponsorships associated with this record
			var sponsorships = context.Sponsorship.Where(t => t.EventSponsorshipId == eventSponsorship.EventSponsorshipId && t.StateCode == 0).ToList();

			// get Event Packages from the Products
			foreach (var curSponsorship in sponsorships)
			{
				if (curSponsorship.EventPackageId.HasValue)
				{
					var curEventPackages =
						context.EventPackage.Where(e => e.EventPackageId == curSponsorship.EventPackageId && e.StateCode == 0).Distinct().ToList();
					eventPackages.AddRange(curEventPackages);
				}
			}
			return eventPackages;
		}



		internal static void UpdateEventPackages(List<EventPackage> eventPackages, PaymentContext context)
		{
			// get rid of duplicate entries
			var distinctEventPackages = new HashSet<EventPackage>(eventPackages);
			Console.WriteLine("Found " + distinctEventPackages.Count + " distinct Event Packages.");
			// go through the event packages and update them
			foreach (var curEventPackage in distinctEventPackages)
			{
				Console.WriteLine("Updating Event Package: " + curEventPackage.EventPackageId);
				UpdateEventPackage(curEventPackage, context);
			}
		}

		internal static void UpdateEventPackage(EventPackage eventPackage, PaymentContext context)
		{
			// get all the Tickets, Products and Sponsorships associated with the Event Package
			var tickets = context.Ticket.Where(t => t.EventPackageId == eventPackage.EventPackageId);
			Console.WriteLine("Found " + tickets.Count() + " Tickets associated with the Event Package");
			var products = context.Product.Where(p => p.EventPackageId == eventPackage.EventPackageId);
			Console.WriteLine("Found " + products.Count() + " Products associated with the Event Package");
			var sponsorships =
				context.Sponsorship.Where(s => s.EventPackageId == eventPackage.EventPackageId);
			Console.WriteLine("Found " + sponsorships.Count() + " Sponsorships associated with the Event Package");

			decimal ticketNonReceiptable = 0;
			decimal productNonReceiptable = 0;
			decimal sponsorshipNonpReceiptable = 0;
			decimal totalNonReceiptable = 0;
			var eventPackageTotalAmount = eventPackage.Amount;

			ticketNonReceiptable = tickets.Sum(t => t.AmountNonreceiptable ?? 0);
			productNonReceiptable = products.Sum(p => p.AmountNonreceiptable ?? 0);
			sponsorshipNonpReceiptable = sponsorships.Sum(s => s.AmountNonreceiptable ?? 0);
			Console.WriteLine("Sum of Ticket Amount Non-Receipetable: " + ticketNonReceiptable);
			Console.WriteLine("Sum of Product Amount ReceiNon-Receipetablepted: " + productNonReceiptable);
			Console.WriteLine("Sum of Sponsorship Amount Non-Receipetable: " + sponsorshipNonpReceiptable);
			totalNonReceiptable = ticketNonReceiptable + productNonReceiptable + sponsorshipNonpReceiptable;
			Console.WriteLine("Total Amount Non-Receipetable:" + totalNonReceiptable);


			eventPackage.AmountNonReceiptable = totalNonReceiptable;
			eventPackage.AmountReceipted = eventPackageTotalAmount - totalNonReceiptable;
			eventPackage.SyncDate = null;
			context.Update(eventPackage);
			context.SaveChanges();
			Console.WriteLine("Updated Event Package: " + eventPackage.EventPackageId);
		}
	}
}
