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
    public class ReceiptLogController : ControllerBase
    {
        private static ReceiptLogWorker _receiptLogWorker;

        public ReceiptLogController(DataFactory dataFactory)
        {
            _receiptLogWorker = (ReceiptLogWorker)dataFactory.GetDataFactory<ReceiptLog>();
        }

        // GET api/ReceiptLog/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var receiptLogRecord = _receiptLogWorker.GetById(id);

            string json = JsonConvert.SerializeObject(receiptLogRecord);

            return json;
        }

        // POST api/ReceiptLog/CreateReceiptLog (Body is JSON)
        [HttpPost]
        [Route("CreateReceiptLog")]
        public HttpResponseMessage CreateReceiptLog(ReceiptLog createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the ReceiptLog record in the Azure SQL DB:
                int receiptLogResult = _receiptLogWorker.UpdateCreate(createRecord);
                if (receiptLogResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (receiptLogResult == 0)
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

        // POST api/ReceiptLog/UpdateReceiptLog (Body is JSON)
        [HttpPost]
        [Route("UpdateReceiptLog")]
        public HttpResponseMessage UpdateReceiptLog(ReceiptLog updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the ReceiptLog record in the Azure SQL DB:
                int receiptLogResult = _receiptLogWorker.UpdateCreate(updateRecord);
                if (receiptLogResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (receiptLogResult == 0)
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

        // DELETE api/ReceiptLog/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _receiptLogWorker.Delete(id);
            }
        }

    }
}