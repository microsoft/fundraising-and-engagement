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
    public class MembershipCategoryController : ControllerBase
    {
        private static MembershipCategoryWorker _membershipCategoryWorker;

        public MembershipCategoryController(DataFactory dataFactory)
        {
            _membershipCategoryWorker = (MembershipCategoryWorker)dataFactory.GetDataFactory<MembershipCategory>();
        }


        // GET api/MembershipCategory/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var retrievedRecord = _membershipCategoryWorker.GetById(id);

            string json = JsonConvert.SerializeObject(retrievedRecord);

            return json;
        }



        // POST api/MembershipCategory/CreateMembershipCategory (Body is JSON)
        [HttpPost]
        [Route("CreateMembershipCategory")]
        public HttpResponseMessage CreateMembershipCategory(MembershipCategory createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int createResult = _membershipCategoryWorker.UpdateCreate(createRecord);
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


        // POST api/MembershipCategory/UpdateMembershipCategory (Body is JSON)
        [HttpPost]
        [Route("UpdateMembershipCategory")]
        public HttpResponseMessage UpdateMembershipCategory(MembershipCategory updatedRecord)
        {
            try
            {
                if (updatedRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int updateResult = _membershipCategoryWorker.UpdateCreate(updatedRecord);
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


        // DELETE api/MembershipCategory/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _membershipCategoryWorker.Delete(id);
            }
        }
    }
}
