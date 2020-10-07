
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
    public class TributeOrMemoryController : ControllerBase
    {
        private static TributeOrMemoryWorker _tributeOrMemoryWorker;

        public TributeOrMemoryController(DataFactory dataFactory)
        {
            _tributeOrMemoryWorker = (TributeOrMemoryWorker)dataFactory.GetDataFactory<TributeOrMemory>();
        }

        // GET api/TributeOrMemory/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var tributeOrMemoryRecord = _tributeOrMemoryWorker.GetById(id);

            string json = JsonConvert.SerializeObject(tributeOrMemoryRecord);

            return json;
        }

        // POST api/TributeOrMemory/CreateTributeOrMemory (Body is JSON)
        [HttpPost]
        [Route("CreateTributeOrMemory")]
        public HttpResponseMessage CreateTributeOrMemory(TributeOrMemory createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the TributeOrMemory record in the Azure SQL DB:
                int tributeOrMemoryResult = _tributeOrMemoryWorker.UpdateCreate(createRecord);
                if (tributeOrMemoryResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (tributeOrMemoryResult == 0)
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

        // POST api/TributeOrMemory/UpdateTributeOrMemory (Body is JSON)
        [HttpPost]
        [Route("UpdateTributeOrMemory")]
        public HttpResponseMessage UpdateTributeOrMemory(TributeOrMemory updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the TributeOrMemory record in the Azure SQL DB:
                int tributeOrMemoryResult = _tributeOrMemoryWorker.UpdateCreate(updateRecord);
                if (tributeOrMemoryResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (tributeOrMemoryResult == 0)
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

        // DELETE api/TributeOrMemory/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _tributeOrMemoryWorker.Delete(id);
            }
        }
    }
}