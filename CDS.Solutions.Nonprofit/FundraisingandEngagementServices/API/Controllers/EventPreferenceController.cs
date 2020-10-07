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
    public class EventPreferenceController : ControllerBase
    {
        private static EventPreferenceWorker _eventPreferenceWorker;

        public EventPreferenceController(DataFactory dataFactory)
        {
            _eventPreferenceWorker = (EventPreferenceWorker)dataFactory.GetDataFactory<EventPreference>();
        }

        // GET api/Event Preference/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var eventPreferenceRecord = _eventPreferenceWorker.GetById(id);

            string json = JsonConvert.SerializeObject(eventPreferenceRecord);

            return json;
        }

        // POST api/Event Preference/CreateEventPreference (Body is JSON)
        [HttpPost]
        [Route("CreateEventPreference")]
        public HttpResponseMessage CreateEventPreference(EventPreference createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Event Preference record in the Azure SQL DB:
                int eventPreferenceResult = _eventPreferenceWorker.UpdateCreate(createRecord);
                if (eventPreferenceResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventPreferenceResult == 0)
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

        // POST api/Event Preference/UpdateEventPreference (Body is JSON)
        [HttpPost]
        [Route("UpdateEventPreference")]
        public HttpResponseMessage UpdateEventPreference(EventPreference updateRecord)
        {
            try
            {
                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Event Preference record in the Azure SQL DB:
                int eventPreferenceResult = _eventPreferenceWorker.UpdateCreate(updateRecord);
                if (eventPreferenceResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventPreferenceResult == 0)
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

        // DELETE api/Event Preference/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _eventPreferenceWorker.Delete(id);
            }
        }
    }
}