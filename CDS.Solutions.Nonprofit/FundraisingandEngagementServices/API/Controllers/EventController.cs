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
    public class EventController : ControllerBase
    {
        private static IFactoryFloor<Event> _eventWorker;

        public EventController(IDataFactory dataFactory)
        {
            _eventWorker = dataFactory.GetDataFactory<Event>();
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