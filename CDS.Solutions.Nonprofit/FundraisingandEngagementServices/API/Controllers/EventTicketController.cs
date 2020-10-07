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
    public class EventTicketController : ControllerBase
    {
        private static EventTicketWorker _eventTicketWorker;

        public EventTicketController(DataFactory dataFactory)
        {
            _eventTicketWorker = (EventTicketWorker)dataFactory.GetDataFactory<EventTicket>();
        }

        // GET api/EventTicket/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var eventTicketRecord = _eventTicketWorker.GetById(id);

            string json = JsonConvert.SerializeObject(eventTicketRecord);

            return json;
        }

        // POST api/EventTicket/CreateEventTicket (Body is JSON)
        [HttpPost]
        [Route("CreateEventTicket")]
        public HttpResponseMessage CreateEventTicket(EventTicket createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the EventTicket record in the Azure SQL DB:
                int eventTicketResult = _eventTicketWorker.UpdateCreate(createRecord);
                if (eventTicketResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventTicketResult == 0)
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

        // POST api/EventTicket/UpdateEventTicket (Body is JSON)
        [HttpPost]
        [Route("UpdateEventTicket")]
        public HttpResponseMessage UpdateEventTicket(EventTicket updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the EventTicket record in the Azure SQL DB:
                int eventTicketResult = _eventTicketWorker.UpdateCreate(updateRecord);
                if (eventTicketResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventTicketResult == 0)
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

        // DELETE api/EventTicket/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _eventTicketWorker.Delete(id);
            }
        }
    }
}