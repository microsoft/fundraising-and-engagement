using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using FundraisingandEngagement.DataFactory;
using FundraisingandEngagement.DataFactory.Workers;
using FundraisingandEngagement.Models.Entities;
using Newtonsoft.Json;

namespace API.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class EventDonationController : ControllerBase
    {
        private static EventDonationWorker _eventDonationWorker;

        public EventDonationController(DataFactory dataFactory)
        {
            _eventDonationWorker = (EventDonationWorker)dataFactory.GetDataFactory<EventDonation>();
        }

        // GET api/EventDonation/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var eventDonationRecord = _eventDonationWorker.GetById(id);

            string json = JsonConvert.SerializeObject(eventDonationRecord);

            return json;
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