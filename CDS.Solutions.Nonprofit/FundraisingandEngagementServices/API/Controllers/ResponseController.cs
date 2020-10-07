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
    public class ResponseController : ControllerBase
    {
        private static ResponseWorker _responseWorker;

        public ResponseController(DataFactory dataFactory)
        {
            _responseWorker = (ResponseWorker)dataFactory.GetDataFactory<Response>();
        }

        // GET api/Response/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var responseRecord = _responseWorker.GetById(id);

            string json = JsonConvert.SerializeObject(responseRecord);

            return json;
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