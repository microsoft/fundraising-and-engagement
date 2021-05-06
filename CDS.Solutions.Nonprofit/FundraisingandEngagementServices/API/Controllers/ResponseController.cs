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
    public class ResponseController : ControllerBase
    {
        private static IFactoryFloor<Response> _responseWorker;

        public ResponseController(IDataFactory dataFactory)
        {
            _responseWorker = dataFactory.GetDataFactory<Response>();
        }

        // POST api/Response/CreateResponse (Body is JSON)
        [HttpPost]
        [Route("CreateResponse")]
        public HttpResponseMessage CreateResponse(Response createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Response record in the Azure SQL DB:
                int responseResult = _responseWorker.UpdateCreate(createRecord);
                if (responseResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (responseResult == 0)
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

        // POST api/Response/UpdateResponse (Body is JSON)
        [HttpPost]
        [Route("UpdateResponse")]
        public HttpResponseMessage UpdateResponse(Response updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Response record in the Azure SQL DB:
                int responseResult = _responseWorker.UpdateCreate(updateRecord);
                if (responseResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (responseResult == 0)
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

        // DELETE api/Response/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _responseWorker.Delete(id);
            }
        }
    }
}