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
    public class RegistrationController : ControllerBase
    {
        private static RegistrationWorker _registrationWorker;

        public RegistrationController(DataFactory dataFactory)
        {
            _registrationWorker = (RegistrationWorker)dataFactory.GetDataFactory<Registration>();
        }

        // GET api/Registration/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var registrationRecord = _registrationWorker.GetById(id);

            string json = JsonConvert.SerializeObject(registrationRecord);

            return json;
        }

        // POST api/Registration/CreateRegistration (Body is JSON)
        [HttpPost]
        [Route("CreateRegistration")]
        public HttpResponseMessage CreateRegistration(Registration createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Registration record in the Azure SQL DB:
                int registrationResult = _registrationWorker.UpdateCreate(createRecord);
                if (registrationResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (registrationResult == 0)
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

        // POST api/Registration/UpdateRegistration (Body is JSON)
        [HttpPost]
        [Route("UpdateRegistration")]
        public HttpResponseMessage UpdateRegistration(Registration updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Registration record in the Azure SQL DB:
                int registrationResult = _registrationWorker.UpdateCreate(updateRecord);
                if (registrationResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (registrationResult == 0)
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

        // DELETE api/Registration/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _registrationWorker.Delete(id);
            }
        }
    }
}