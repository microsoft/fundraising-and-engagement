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
    public class PreferenceCategoryController : ControllerBase
    {
        private static IFactoryFloor<PreferenceCategory> _preferenceCategoryWorker;

        public PreferenceCategoryController(IDataFactory dataFactory)
        {
            _preferenceCategoryWorker = dataFactory.GetDataFactory<PreferenceCategory>();
        }

        // POST api/Preference Category/CreatePreferenceCategory (Body is JSON)
        [HttpPost]
        [Route("CreatePreferenceCategory")]
        public HttpResponseMessage CreatePreferenceCategory(PreferenceCategory createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Preference Category record in the Azure SQL DB:
                int preferenceCategoryResult = _preferenceCategoryWorker.UpdateCreate(createRecord);
                if (preferenceCategoryResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (preferenceCategoryResult == 0)
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

        // POST api/Preference Category/UpdatePreferenceCategory (Body is JSON)
        [HttpPost]
        [Route("UpdatePreferenceCategory")]
        public HttpResponseMessage UpdatePreferenceCategory(PreferenceCategory updateRecord)
        {
            try
            {
                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Preference Category record in the Azure SQL DB:
                int preferenceCategoryResult = _preferenceCategoryWorker.UpdateCreate(updateRecord);
                if (preferenceCategoryResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (preferenceCategoryResult == 0)
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

        // DELETE api/Preference Category/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _preferenceCategoryWorker.Delete(id);
            }
        }
    }
}