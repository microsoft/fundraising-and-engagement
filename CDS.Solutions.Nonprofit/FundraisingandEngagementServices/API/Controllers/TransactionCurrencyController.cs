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
    public class TransactionCurrencyController : ControllerBase
    {
        private static IFactoryFloor<TransactionCurrency> _transactionCurrencyWorker;

        public TransactionCurrencyController(IDataFactory dataFactory)
        {
            _transactionCurrencyWorker = dataFactory.GetDataFactory<TransactionCurrency>();
        }

        // POST api/TransactionCurrency/CreateTransactionCurrency (Body is JSON)
        [HttpPost]
        [Route("CreateTransactionCurrency")]
        public HttpResponseMessage CreateTransactionCurrency(TransactionCurrency createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int createResult = _transactionCurrencyWorker.UpdateCreate(createRecord);
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


        // POST api/TransactionCurrency/UpdateTransactionCurrency (Body is JSON)
        [HttpPost]
        [Route("UpdateTransactionCurrency")]
        public HttpResponseMessage UpdateTransactionCurrency(TransactionCurrency updatedRecord)
        {
            try
            {
                if (updatedRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int updateResult = _transactionCurrencyWorker.UpdateCreate(updatedRecord);
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


        // DELETE api/TransactionCurrency/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _transactionCurrencyWorker.Delete(id);
            }
        }
    }
}