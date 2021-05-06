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
    public class EventProductController : ControllerBase
    {
        private static IFactoryFloor<EventProduct> _eventProductWorker;

        public EventProductController(IDataFactory dataFactory)
        {
            _eventProductWorker = dataFactory.GetDataFactory<EventProduct>();
        }

        // POST api/EventProduct/CreateEventProduct (Body is JSON)
        [HttpPost]
        [Route("CreateEventProduct")]
        public HttpResponseMessage CreateEventProduct(EventProduct createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the EventProduct record in the Azure SQL DB:
                int eventProductResult = _eventProductWorker.UpdateCreate(createRecord);
                if (eventProductResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventProductResult == 0)
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

        // POST api/EventProduct/UpdateEventProduct (Body is JSON)
        [HttpPost]
        [Route("UpdateEventProduct")]
        public HttpResponseMessage UpdateEventProduct(EventProduct updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the EventProduct record in the Azure SQL DB:
                int eventProductResult = _eventProductWorker.UpdateCreate(updateRecord);
                if (eventProductResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (eventProductResult == 0)
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

        // DELETE api/EventProduct/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _eventProductWorker.Delete(id);
            }
        }
    }
}