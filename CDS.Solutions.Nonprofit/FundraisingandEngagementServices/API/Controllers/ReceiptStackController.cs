using System;
using Microsoft.AspNetCore.Mvc;
using FundraisingandEngagement.Models.Entities;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using FundraisingandEngagement.DataFactory;
using FundraisingandEngagement.DataFactory.Workers;

namespace API.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class ReceiptStackController : ControllerBase
    {
        private static ReceiptStackWorker _receiptStackWorker;

        public ReceiptStackController(DataFactory dataFactory)
        {
            _receiptStackWorker = (ReceiptStackWorker)dataFactory.GetDataFactory<ReceiptStack>();
        }


        // GET api/ReceiptStack/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var retrievedRecord = _receiptStackWorker.GetById(id);

            string json = JsonConvert.SerializeObject(retrievedRecord);

            return json;
        }



        // POST api/ReceiptStack/CreateReceiptStack (Body is JSON)
        [HttpPost]
        [Route("CreateReceiptStack")]
        public HttpResponseMessage CreateReceiptStack(ReceiptStack createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int createResult = _receiptStackWorker.UpdateCreate(createRecord);
                if (createResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (createResult == 0)
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


        // POST api/ReceiptStack/UpdateReceiptStack (Body is JSON)
        [HttpPost]
        [Route("UpdateReceiptStack")]
        public HttpResponseMessage UpdateReceiptStack(ReceiptStack updatedRecord)
        {
            try
            {
                if (updatedRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int updateResult = _receiptStackWorker.UpdateCreate(updatedRecord);
                if (updateResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (updateResult == 0)
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


        // DELETE api/ReceiptStack/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _receiptStackWorker.Delete(id);
            }
        }
    }
}
