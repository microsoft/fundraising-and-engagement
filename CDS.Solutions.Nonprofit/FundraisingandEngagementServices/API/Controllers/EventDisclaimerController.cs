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
    public class EventDisclaimerController : ControllerBase
    {
        private static EventDisclaimerWorker _eventDisclaimerWorker;

        public EventDisclaimerController(DataFactory dataFactory)
        {
            _eventDisclaimerWorker = (EventDisclaimerWorker)dataFactory.GetDataFactory<EventDisclaimer>();
        }

        // GET api/EventDisclaimer/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var eventDisclaimerRecord = _eventDisclaimerWorker.GetById(id);

            string json = JsonConvert.SerializeObject(eventDisclaimerRecord);

            return json;
        }

        // POST api/EventDisclaimer/CreateEventDisclaimer (Body is JSON)
        [HttpPost]
        [Route("CreateEventDisclaimer")]
        public HttpResponseMessage CreateEventDisclaimer(EventDisclaimer createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the EventDisclaimer record in the Azure SQL DB:
                int eventDisclaimerResult = _eventDisclaimerWorker.UpdateCreate(createRecord);
                if (eventDisclaimerResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventDisclaimerResult == 0)
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

        // POST api/EventDisclaimer/UpdateEventDisclaimer (Body is JSON)
        [HttpPost]
        [Route("UpdateEventDisclaimer")]
        public HttpResponseMessage UpdateEventDisclaimer(EventDisclaimer updateRecord)
        {
            try
            {
                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the EventDisclaimer record in the Azure SQL DB:
                int eventDisclaimerResult = _eventDisclaimerWorker.UpdateCreate(updateRecord);
                if (eventDisclaimerResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventDisclaimerResult == 0)
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

        // DELETE api/EventDisclaimer/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _eventDisclaimerWorker.Delete(id);
            }
        }
    }
}