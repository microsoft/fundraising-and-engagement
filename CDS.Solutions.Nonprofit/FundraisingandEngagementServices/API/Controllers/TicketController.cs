using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.DataFactory;
using FundraisingandEngagement.DataFactory.Workers;
using Newtonsoft.Json;

namespace API.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private static TicketWorker _ticketWorker;

        public TicketController(DataFactory dataFactory)
        {
            _ticketWorker = (TicketWorker)dataFactory.GetDataFactory<Ticket>();
        }

        // GET api/Ticket/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var ticketRecord = _ticketWorker.GetById(id);

            string json = JsonConvert.SerializeObject(ticketRecord);

            return json;
        }

        // POST api/Ticket/CreateTicket (Body is JSON)
        [HttpPost]
        [Route("CreateTicket")]
        public HttpResponseMessage CreateTicket(Ticket createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Ticket record in the Azure SQL DB:
                int ticketResult = _ticketWorker.UpdateCreate(createRecord);
                if (ticketResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (ticketResult == 0)
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

        // POST api/Ticket/UpdateTicket (Body is JSON)
        [HttpPost]
        [Route("UpdateTicket")]
        public HttpResponseMessage UpdateTicket(Ticket updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Ticket record in the Azure SQL DB:
                int ticketResult = _ticketWorker.UpdateCreate(updateRecord);
                if (ticketResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (ticketResult == 0)
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

        // DELETE api/Ticket/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _ticketWorker.Delete(id);
            }
        }
    }
}