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
    public class MembershipOrderController : ControllerBase
    {
        private static IFactoryFloor<MembershipOrder> _membershipOrderWorker;

        public MembershipOrderController(IDataFactory dataFactory)
        {
            _membershipOrderWorker = dataFactory.GetDataFactory<MembershipOrder>();
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