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
    public class EventTableController : ControllerBase
    {
        private static IFactoryFloor<EventTable> _eventTableWorker;

        public EventTableController(IDataFactory dataFactory)
        {
            _eventTableWorker = dataFactory.GetDataFactory<EventTable>();
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