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
    public class TransactionController : ControllerBase
    {
        private static TransactionWorker _transactionWorker;

        public TransactionController(DataFactory dataFactory)
        {
            _transactionWorker = (TransactionWorker)dataFactory.GetDataFactory<Transaction>();
            var _paymentScheduleWorker = (PaymentScheduleWorker)dataFactory.GetDataFactory<PaymentSchedule>();
        }


        // GET api/transaction/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if(id == null)
            {
                return "";
            }

            var transactionRecord = _transactionWorker.GetById(id);

            string json = JsonConvert.SerializeObject(transactionRecord);

            return json;
        }

        [HttpGet]
        [Route("Retrieve")]
        public ActionResult Retrieve(string campaignId = "", string appealId = "", string packageId = "", string dateFrom = "", string dateTo = "", string cashPaymentCode = "", string paymentTypeCode = "", string preferredLanguageCode = "", string donorSegmentationCode = "", string addressPresentCode = "", string businessUnit = "")
        {
            return Ok(_transactionWorker.RetrieveWithCriteria(campaignId, appealId, packageId, dateFrom, dateTo, cashPaymentCode, paymentTypeCode, preferredLanguageCode, donorSegmentationCode, addressPresentCode, businessUnit));
        }

        // POST api/transaction/CreateTransaction (Body is JSON)
        [HttpPost]
        [Route("CreateTransaction")]
        public HttpResponseMessage CreateTransaction(Transaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the transaction record in the Azure SQL DB:
                int transactionResult = _transactionWorker.UpdateCreate(transaction);
                if (transactionResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (transactionResult == 0)
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


        // POST api/transaction/UpdateTransaction (Body is JSON)
        [HttpPost]
        [Route("UpdateTransaction")]
        public HttpResponseMessage UpdateTransaction(Transaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the transaction record in the Azure SQL DB:
                int transactionResult = _transactionWorker.UpdateCreate(transaction);
                if (transactionResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (transactionResult == 0)
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

               
        // DELETE api/transaction/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _transactionWorker.Delete(id);
            }
        }
    }
}
