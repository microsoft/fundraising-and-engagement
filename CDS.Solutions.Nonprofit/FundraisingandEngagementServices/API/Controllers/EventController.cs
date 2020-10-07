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
    public class EventController : ControllerBase
    {
        private static EventWorker _eventWorker;

        public EventController(DataFactory dataFactory)
        {
            _eventWorker = (EventWorker)dataFactory.GetDataFactory<Event>();
        }

        // GET api/Event/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var eventRecord = _eventWorker.GetById(id);

            string json = JsonConvert.SerializeObject(eventRecord);

            return json;
        }

        // POST api/Event/CreateEvent (Body is JSON)
        [HttpPost]
        [Route("CreateEvent")]
        public HttpResponseMessage CreateEvent(Event createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Event record in the Azure SQL DB:
                int eventResult = _eventWorker.UpdateCreate(createRecord);
                if (eventResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventResult == 0)
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

        // POST api/Event/UpdateEvent (Body is JSON)
        [HttpPost]
        [Route("UpdateEvent")]
        public HttpResponseMessage UpdateEvent(Event updateRecord)
        {
            try
            {
                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Event record in the Azure SQL DB:
                int eventResult = _eventWorker.UpdateCreate(updateRecord);
                if (eventResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventResult == 0)
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

        // DELETE api/Event/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _eventWorker.Delete(id);
            }
        }
    }
}