﻿using System;
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
    public class EventSponsorshipController : ControllerBase
    {
        private static IFactoryFloor<EventSponsorship> _eventSponsorshipWorker;

        public EventSponsorshipController(IDataFactory dataFactory)
        {
            _eventSponsorshipWorker = dataFactory.GetDataFactory<EventSponsorship>();
        }

        // POST api/EventSponsorship/CreateEventSponsorship (Body is JSON)
        [HttpPost]
        [Route("CreateEventSponsorship")]
        public HttpResponseMessage CreateEventSponsorship(EventSponsorship createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the EventSponsorship record in the Azure SQL DB:
                int eventSponsorshipResult = _eventSponsorshipWorker.UpdateCreate(createRecord);
                if (eventSponsorshipResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventSponsorshipResult == 0)
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

        // POST api/EventSponsorship/UpdateEventSponsorship (Body is JSON)
        [HttpPost]
        [Route("UpdateEventSponsorship")]
        public HttpResponseMessage UpdateEventSponsorship(EventSponsorship updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the EventSponsorship record in the Azure SQL DB:
                int eventSponsorshipResult = _eventSponsorshipWorker.UpdateCreate(updateRecord);
                if (eventSponsorshipResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventSponsorshipResult == 0)
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

        // DELETE api/EventSponsorship/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _eventSponsorshipWorker.Delete(id);
            }
        }
    }
}