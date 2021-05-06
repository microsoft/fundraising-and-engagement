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
    public class ConfigurationController : ControllerBase
    {
        private static IFactoryFloor<Configuration> _configurationWorker;

        public ConfigurationController(IDataFactory dataFactory)
        {
            _configurationWorker = dataFactory.GetDataFactory<Configuration>();
        }


        // POST api/configuration/CreateConfiguration (Body is JSON)
        [HttpPost]
        [Route("CreateConfiguration")]
        public HttpResponseMessage CreateConfiguration(Configuration configuration)
        {
            try
            {
                if (configuration == null)
                {
                    //return new ContentResult
                    //{
                    //    Content = "Input null",
                    //    ContentType = "text/plain",
                    //    StatusCode = (int)HttpStatusCode.BadRequest
                    //};
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the record in the Azure SQL DB:
                int createResult = _configurationWorker.UpdateCreate(configuration);

                //return new ContentResult
                //{
                //    StatusCode = (int)HttpStatusCode.OK
                //};
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {

                //return new ContentResult
                //{
                //    Content = e.Message,
                //    ContentType = "text/plain",
                //    StatusCode = (int)HttpStatusCode.InternalServerError
                //};
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent(e.Message);
                return response;
            }
        }


        // POST api/configuration/UpdateConfiguration (Body is JSON)
        [HttpPost]
        [Route("UpdateConfiguration")]
        public HttpResponseMessage UpdateConfiguration(Configuration configuration)
        {
            try
            {
                if (configuration == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the record in the Azure SQL DB:
                int updateResult = _configurationWorker.UpdateCreate(configuration);
                if (updateResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }


                return new HttpResponseMessage(HttpStatusCode.OK);

            }
            catch (Exception e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent(e.Message);
                return response;
            }
        }


        // DELETE api/configuration/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _configurationWorker.Delete(id);
            }
        }
    }
}