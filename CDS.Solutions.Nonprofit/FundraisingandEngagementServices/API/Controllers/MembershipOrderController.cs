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
    public class MembershipOrderController : ControllerBase
    {
        private static MembershipOrderWorker _membershipOrderWorker;

        public MembershipOrderController(DataFactory dataFactory)
        {
            _membershipOrderWorker = (MembershipOrderWorker)dataFactory.GetDataFactory<MembershipOrder>();
        }


        // GET api/MembershipOrder/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var retrievedRecord = _membershipOrderWorker.GetById(id);

            string json = JsonConvert.SerializeObject(retrievedRecord);

            return json;
        }



        // POST api/MembershipOrder/CreateMembershipOrder (Body is JSON)
        [HttpPost]
        [Route("CreateMembershipOrder")]
        public HttpResponseMessage CreateMembershipOrder(MembershipOrder createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int createResult = _membershipOrderWorker.UpdateCreate(createRecord);
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


        // POST api/MembershipOrder/UpdateMembershipOrder (Body is JSON)
        [HttpPost]
        [Route("UpdateMembershipOrder")]
        public HttpResponseMessage UpdateMembershipOrder(MembershipOrder updatedRecord)
        {
            try
            {
                if (updatedRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int updateResult = _membershipOrderWorker.UpdateCreate(updatedRecord);
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


        // DELETE api/MembershipOrder/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _membershipOrderWorker.Delete(id);
            }
        }
    }
}
