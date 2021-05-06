using System;
using System.Net;
using System.Net.Http;
using FundraisingandEngagement.DataFactory;
using FundraisingandEngagement.DataFactory.Workers;
using FundraisingandEngagement.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventDonationController : ControllerBase
    {
        private static IFactoryFloor<EventDonation> _eventDonationWorker;

        public EventDonationController(IDataFactory dataFactory)
        {
            _eventDonationWorker = dataFactory.GetDataFactory<EventDonation>();
        }

        // POST api/EventDonation/CreateEventDonation (Body is JSON)
        [HttpPost]
        [Route("CreateEventDonation")]
        public HttpResponseMessage CreateEventDonation(EventDonation createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the EventDonation record in the Azure SQL DB:
                int eventDonationResult = _eventDonationWorker.UpdateCreate(createRecord);
                if (eventDonationResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventDonationResult == 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }

                return new HttpResponseMessage(HttpStatusCode.OK);

            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        // POST api/EventDonation/UpdateEventDonation (Body is JSON)
        [HttpPost]
        [Route("UpdateEventDonation")]
        public HttpResponseMessage UpdateEventDonation(EventDonation updateRecord)
        {
            try
            {
                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the EventDonation record in the Azure SQL DB:
                int eventDonationResult = _eventDonationWorker.UpdateCreate(updateRecord);
                if (eventDonationResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventDonationResult == 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }

                return new HttpResponseMessage(HttpStatusCode.OK);

            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/EventDonation/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _eventDonationWorker.Delete(id);
            }
        }
    }
}