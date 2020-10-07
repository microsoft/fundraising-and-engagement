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
    public class EventSponsorController : ControllerBase
    {
        private static EventSponsorWorker _eventSponsorWorker;

        public EventSponsorController(DataFactory dataFactory)
        {
            _eventSponsorWorker = (EventSponsorWorker)dataFactory.GetDataFactory<EventSponsor>();
        }

        // GET api/EventSponsor/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var eventSponsorRecord = _eventSponsorWorker.GetById(id);

            string json = JsonConvert.SerializeObject(eventSponsorRecord);

            return json;
        }

        // POST api/EventSponsor/CreateEventSponsor (Body is JSON)
        [HttpPost]
        [Route("CreateEventSponsor")]
        public HttpResponseMessage CreateEventSponsor(EventSponsor createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the EventSponsor record in the Azure SQL DB:
                int eventSponsorResult = _eventSponsorWorker.UpdateCreate(createRecord);
                if (eventSponsorResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventSponsorResult == 0)
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

        // POST api/EventSponsor/UpdateEventSponsor (Body is JSON)
        [HttpPost]
        [Route("UpdateEventSponsor")]
        public HttpResponseMessage UpdateEventSponsor(EventSponsor updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the EventSponsor record in the Azure SQL DB:
                int eventSponsorResult = _eventSponsorWorker.UpdateCreate(updateRecord);
                if (eventSponsorResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventSponsorResult == 0)
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

        // DELETE api/EventSponsor/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _eventSponsorWorker.Delete(id);
            }
        }
    }
}