using System;
using System.Net;
using System.Net.Http;
using FundraisingandEngagement.DataFactory;
using FundraisingandEngagement.DataFactory.Workers;
using FundraisingandEngagement.Models.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class DesignationController : ControllerBase
    {
        private static IFactoryFloor<Designation> _DesignationWorker;

        public DesignationController(IDataFactory dataFactory)
        {
            _DesignationWorker = dataFactory.GetDataFactory<Designation>();
        }

        // POST api/Designation/CreateDesignation (Body is JSON)
        [HttpPost]
        [Route("CreateDesignation")]
        public HttpResponseMessage CreateDesignation(Designation createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Designation record in the Azure SQL DB:
                int DesignationResult = _DesignationWorker.UpdateCreate(createRecord);
                if (DesignationResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (DesignationResult == 0)
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

        // POST api/Designation/UpdateDesignation (Body is JSON)
        [HttpPost]
        [Route("UpdateDesignation")]
        public HttpResponseMessage UpdateDesignation(Designation updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Designation record in the Azure SQL DB:
                int DesignationResult = _DesignationWorker.UpdateCreate(updateRecord);
                if (DesignationResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (DesignationResult == 0)
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

        // DELETE api/Designation/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _DesignationWorker.Delete(id);
            }
        }
    }
}