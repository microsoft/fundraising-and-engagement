using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using FundraisingandEngagement.DataFactory;
using FundraisingandEngagement.DataFactory.Workers;
using FundraisingandEngagement.Models.Entities;
using Newtonsoft.Json;

namespace API.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private static AccountWorker _accountWorker;

        public AccountController(DataFactory dataFactory)
        {
            _accountWorker = (AccountWorker)dataFactory.GetDataFactory<Account>();
        }

        // GET api/Account/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var retrievedRecord = _accountWorker.GetById(id);

            string json = JsonConvert.SerializeObject(retrievedRecord);

            return json;
        }



        // POST api/Account/CreateAccount (Body is JSON)
        [HttpPost]
        [Route("CreateAccount")]
        public HttpResponseMessage CreateAccount(Account createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int createResult = _accountWorker.UpdateCreate(createRecord);
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


        // POST api/Account/UpdateAccount (Body is JSON)
        [HttpPost]
        [Route("UpdateAccount")]
        public HttpResponseMessage UpdateAccount(Account updatedRecord)
        {
            try
            {
                if (updatedRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int updateResult = _accountWorker.UpdateCreate(updatedRecord);
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


        // DELETE api/Account/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _accountWorker.Delete(id);
            }
        }
    }
}