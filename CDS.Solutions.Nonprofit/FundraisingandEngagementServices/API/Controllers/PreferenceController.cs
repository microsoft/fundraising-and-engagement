﻿using System;
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
    public class PreferenceController : ControllerBase
    {
        private static IFactoryFloor<Preference> _preferenceWorker;

        public PreferenceController(IDataFactory dataFactory)
        {
            _preferenceWorker = dataFactory.GetDataFactory<Preference>();
        }

        // POST api/Preference/CreatePreference (Body is JSON)
        [HttpPost]
        [Route("CreatePreference")]
        public HttpResponseMessage CreatePreference(Preference createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Preference record in the Azure SQL DB:
                int preferenceResult = _preferenceWorker.UpdateCreate(createRecord);
                if (preferenceResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (preferenceResult == 0)
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

        // POST api/Preference/UpdatePreference (Body is JSON)
        [HttpPost]
        [Route("UpdatePreference")]
        public HttpResponseMessage UpdatePreference(Preference updateRecord)
        {
            try
            {
                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Preference record in the Azure SQL DB:
                int preferenceResult = _preferenceWorker.UpdateCreate(updateRecord);
                if (preferenceResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (preferenceResult == 0)
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

        // DELETE api/Preference/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _preferenceWorker.Delete(id);
            }
        }
    }
}