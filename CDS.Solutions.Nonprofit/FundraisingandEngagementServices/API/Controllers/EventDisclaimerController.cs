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
    public class EventDisclaimerController : ControllerBase
    {
        private static IFactoryFloor<EventDisclaimer> _eventDisclaimerWorker;

        public EventDisclaimerController(IDataFactory dataFactory)
        {
            _eventDisclaimerWorker = dataFactory.GetDataFactory<EventDisclaimer>();
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