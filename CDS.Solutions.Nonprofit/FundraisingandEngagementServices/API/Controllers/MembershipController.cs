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
    public class MembershipController : ControllerBase
    {
        private static MembershipWorker _membershipWorker;

        public MembershipController(DataFactory dataFactory)
        {
            _membershipWorker = (MembershipWorker)dataFactory.GetDataFactory<Membership>();
        }


        // GET api/Membership/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var retrievedRecord = _membershipWorker.GetById(id);

            string json = JsonConvert.SerializeObject(retrievedRecord);

            return json;
        }



        // POST api/Membership/CreateMembership (Body is JSON)
        [HttpPost]
        [Route("CreateMembership")]
        public HttpResponseMessage CreateMembership(Membership createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int createResult = _membershipWorker.UpdateCreate(createRecord);
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


        // POST api/Membership/UpdateMembership (Body is JSON)
        [HttpPost]
        [Route("UpdateMembership")]
        public HttpResponseMessage UpdateMembership(Membership updatedRecord)
        {
            try
            {
                if (updatedRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int updateResult = _membershipWorker.UpdateCreate(updatedRecord);
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


        // DELETE api/Membership/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _membershipWorker.Delete(id);
            }
        }
    }
}
