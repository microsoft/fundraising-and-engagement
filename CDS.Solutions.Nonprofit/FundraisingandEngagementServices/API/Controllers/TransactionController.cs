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
    public class TransactionController : ControllerBase
    {
        private static IFactoryFloor<Transaction> _transactionWorker;

        public TransactionController(IDataFactory dataFactory)
        {
            _transactionWorker = dataFactory.GetDataFactory<Transaction>();
            var _paymentScheduleWorker = dataFactory.GetDataFactory<PaymentSchedule>();
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