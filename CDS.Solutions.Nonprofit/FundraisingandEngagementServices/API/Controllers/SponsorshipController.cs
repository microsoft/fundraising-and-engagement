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
    public class SponsorshipController : ControllerBase
    {
        private static SponsorshipWorker _sponsorshipWorker;

        public SponsorshipController(DataFactory dataFactory)
        {
            _sponsorshipWorker = (SponsorshipWorker)dataFactory.GetDataFactory<Sponsorship>();
        }

        // GET api/Sponsorship/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var sponsorshipRecord = _sponsorshipWorker.GetById(id);

            string json = JsonConvert.SerializeObject(sponsorshipRecord);

            return json;
        }

        // POST api/Sponsorship/CreateSponsorship (Body is JSON)
        [HttpPost]
        [Route("CreateSponsorship")]
        public HttpResponseMessage CreateSponsorship(Sponsorship createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Sponsorship record in the Azure SQL DB:
                int sponsorshipResult = _sponsorshipWorker.UpdateCreate(createRecord);
                if (sponsorshipResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (sponsorshipResult == 0)
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

        // POST api/Sponsorship/UpdateSponsorship (Body is JSON)
        [HttpPost]
        [Route("UpdateSponsorship")]
        public HttpResponseMessage UpdateSponsorship(Sponsorship updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Sponsorship record in the Azure SQL DB:
                int sponsorshipResult = _sponsorshipWorker.UpdateCreate(updateRecord);
                if (sponsorshipResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (sponsorshipResult == 0)
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

        // DELETE api/Sponsorship/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _sponsorshipWorker.Delete(id);
            }
        }
    }
}