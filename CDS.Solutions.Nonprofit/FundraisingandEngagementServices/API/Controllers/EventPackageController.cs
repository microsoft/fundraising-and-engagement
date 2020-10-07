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
    public class EventPackageController : ControllerBase
    {
        private static EventPackageWorker _eventPackageWorker;

        public EventPackageController(DataFactory dataFactory)
        {
            _eventPackageWorker = (EventPackageWorker)dataFactory.GetDataFactory<EventPackage>();
        }

        // GET api/EventPackage/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var eventPackageRecord = _eventPackageWorker.GetById(id);

            string json = JsonConvert.SerializeObject(eventPackageRecord);

            return json;
        }

        // POST api/EventPackage/CreateEventPackage (Body is JSON)
        [HttpPost]
        [Route("CreateEventPackage")]
        public HttpResponseMessage CreateEventPackage(EventPackage createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the EventPackage record in the Azure SQL DB:
                int eventPackageResult = _eventPackageWorker.UpdateCreate(createRecord);
                if (eventPackageResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventPackageResult == 0)
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

        // POST api/EventPackage/UpdateEventPackage (Body is JSON)
        [HttpPost]
        [Route("UpdateEventPackage")]
        public HttpResponseMessage UpdateEventPackage(EventPackage updateRecord)
        {
            try
            {
                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the EventPackage record in the Azure SQL DB:
                int eventPackageResult = _eventPackageWorker.UpdateCreate(updateRecord);
                if (eventPackageResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventPackageResult == 0)
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

        // DELETE api/EventPackage/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _eventPackageWorker.Delete(id);
            }
        }
    }
}