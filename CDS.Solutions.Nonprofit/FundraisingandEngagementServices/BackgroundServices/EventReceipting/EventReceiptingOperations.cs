using System;
using System.Collections.Generic;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Data.DataSynchronization;
using FundraisingandEngagement.Models.Entities;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
    public class EventReceiptingOperations
    {
        private readonly PaymentContext _context;
        private readonly ILogger _logger;
        private readonly IDbEntitySynchronizer _dbEntitySynchronizer;

        public EventReceiptingOperations(PaymentContext paymentContext, ILogger logger, IDbEntitySynchronizer dbEntitySynchronizer)
        {
            this._context = paymentContext;
            this._logger = logger;
            this._dbEntitySynchronizer = dbEntitySynchronizer;
        }

        public void ExecuteEventReceipting(Guid entityId, string entityName)
        {
            this._logger.LogInformation("----------Entering ExecuteEventReceipting()----------");

            try
            {
                _dbEntitySynchronizer.SynchronizeEntitiesToDbTransitively(new List<Type>
                {
                    typeof(EventPackage),
                    typeof(EventTicket),
                    typeof(Event),
                    typeof(EventProduct),
                    typeof(Product),
                    typeof(EventSponsorship),
                    typeof(Sponsorship)
                });

                List<EventPackage> eventPackages;
                switch (entityName)
                {
                    case EventReceipting.EventTicket:
                        EventTicket eventTicket = EventReceipting.GetEventTicketFromId(entityId, this._context);
                        EventReceipting.UpdateTicketsFromEventTicket(eventTicket, this._context);
                        eventPackages = EventReceipting.GetEventPackagesFromEventTicket(eventTicket, this._context);
                        break;

                    case EventReceipting.EventProduct:
                        EventProduct eventProduct = EventReceipting.GetEventProductFromId(entityId, this._context);
                        EventReceipting.UpdateProductsFromEventProduct(eventProduct, this._context);
                        eventPackages = EventReceipting.GetEventPackagesFromEventProduct(eventProduct, this._context);
                        break;

                    case EventReceipting.EventSponsorship:
                        EventSponsorship eventSponsorship = EventReceipting.GetEventSponsorshipFromId(entityId, this._context);
                        EventReceipting.UpdateSponsorshipsFromEventSponsorship(eventSponsorship, this._context);
                        eventPackages = EventReceipting.GetEventPackagesFromEventSponsorship(eventSponsorship, this._context);
                        break;
                    default:
                        throw new Exception("Unknown Entity for Event Receipting: " + entityName + ". Exiting.");
                }

                EventReceipting.UpdateEventPackages(eventPackages, this._context);
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "Error in ExecuteEventReceipting(): " + e.Message);
                throw;
            }

            this._logger.LogInformation("----------Exiting ExecuteEventReceipting()----------");
        }
    }
}