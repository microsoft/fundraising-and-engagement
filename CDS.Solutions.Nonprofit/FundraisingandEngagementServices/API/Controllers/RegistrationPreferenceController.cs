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
    public class RegistrationPreferenceController : ControllerBase
    {
        private static RegistrationPreferenceWorker _registrationPreferenceWorker;

        public RegistrationPreferenceController(DataFactory dataFactory)
        {
            _registrationPreferenceWorker = (RegistrationPreferenceWorker)dataFactory.GetDataFactory<RegistrationPreference>();
        }

        // GET api/Registration Preference/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var registrationPreferenceRecord = _registrationPreferenceWorker.GetById(id);

            string json = JsonConvert.SerializeObject(registrationPreferenceRecord);

            return json;
        }

        // POST api/Registration Preference/CreateRegistrationPreference (Body is JSON)
        [HttpPost]
        [Route("CreateRegistrationPreference")]
        public HttpResponseMessage CreateRegistrationPreference(RegistrationPreference createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Registration Preference record in the Azure SQL DB:
                int registrationPreferenceResult = _registrationPreferenceWorker.UpdateCreate(createRecord);
                if (registrationPreferenceResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (registrationPreferenceResult == 0)
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

        // POST api/Registration Preference/UpdateRegistrationPreference (Body is JSON)
        [HttpPost]
        [Route("UpdateRegistrationPreference")]
        public HttpResponseMessage UpdateRegistrationPreference(RegistrationPreference updateRecord)
        {
            try
            {
                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Registration Preference record in the Azure SQL DB:
                int registrationPreferenceResult = _registrationPreferenceWorker.UpdateCreate(updateRecord);
                if (registrationPreferenceResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (registrationPreferenceResult == 0)
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

        // DELETE api/Registration Preference/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _registrationPreferenceWorker.Delete(id);
            }
        }
    }
}