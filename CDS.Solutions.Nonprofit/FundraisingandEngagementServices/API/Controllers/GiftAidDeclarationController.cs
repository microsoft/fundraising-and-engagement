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
    public class GiftAidDeclarationController : ControllerBase
    {
        private static GiftAidDeclarationWorker _GiftAidDeclarationWorker;

        public GiftAidDeclarationController(DataFactory dataFactory)
        {
            _GiftAidDeclarationWorker = (GiftAidDeclarationWorker)dataFactory.GetDataFactory<GiftAidDeclaration>();
        }


        // GET api/GiftAidDeclaration/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var GiftAidDeclarationRecord = _GiftAidDeclarationWorker.GetById(id);

            string json = JsonConvert.SerializeObject(GiftAidDeclarationRecord);

            return json;
        }



        // POST api/GiftAidDeclaration/CreateGiftAidDeclaration (Body is JSON)
        [HttpPost]
        [Route("CreateGiftAidDeclaration")]
        public HttpResponseMessage CreateGiftAidDeclaration(GiftAidDeclaration GiftAidDeclaration)
        {
            try
            {
                if (GiftAidDeclaration == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the GiftAidDeclaration record in the Azure SQL DB:
                int GiftAidDeclarationResult = _GiftAidDeclarationWorker.UpdateCreate(GiftAidDeclaration);
                if (GiftAidDeclarationResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (GiftAidDeclarationResult == 0)
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


        // POST api/GiftAidDeclaration/UpdateGiftAidDeclaration (Body is JSON)
        [HttpPost]
        [Route("UpdateGiftAidDeclaration")]
        public HttpResponseMessage UpdateGiftAidDeclaration(GiftAidDeclaration GiftAidDeclaration)
        {
            try
            {
                if (GiftAidDeclaration == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the GiftAidDeclaration record in the Azure SQL DB:
                int GiftAidDeclarationResult = _GiftAidDeclarationWorker.UpdateCreate(GiftAidDeclaration);
                if (GiftAidDeclarationResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (GiftAidDeclarationResult == 0)
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


        // DELETE api/GiftAidDeclaration/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _GiftAidDeclarationWorker.Delete(id);
            }
        }
    }
}
