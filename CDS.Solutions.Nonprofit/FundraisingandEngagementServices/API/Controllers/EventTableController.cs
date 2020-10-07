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
    public class EventTableController : ControllerBase
    {
        private static EventTableWorker _eventTableWorker;

        public EventTableController(DataFactory dataFactory)
        {
            _eventTableWorker = (EventTableWorker)dataFactory.GetDataFactory<EventTable>();
        }

        // GET api/Event Table/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var eventTableRecord = _eventTableWorker.GetById(id);

            string json = JsonConvert.SerializeObject(eventTableRecord);

            return json;
        }

        // POST api/Event Table/CreateEventTable (Body is JSON)
        [HttpPost]
        [Route("CreateEventTable")]
        public HttpResponseMessage CreateEventTable(EventTable createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Event Table record in the Azure SQL DB:
                int eventTableResult = _eventTableWorker.UpdateCreate(createRecord);
                if (eventTableResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventTableResult == 0)
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

        // POST api/Event Table/UpdateEventTable (Body is JSON)
        [HttpPost]
        [Route("UpdateEventTable")]
        public HttpResponseMessage UpdateEventTable(EventTable updateRecord)
        {
            try
            {
                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Event Table record in the Azure SQL DB:
                int eventTableResult = _eventTableWorker.UpdateCreate(updateRecord);
                if (eventTableResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventTableResult == 0)
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

        // DELETE api/Event Table/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _eventTableWorker.Delete(id);
            }
        }
    }
}