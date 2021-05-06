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
    public class GiftAidDeclarationController : ControllerBase
    {
        private static IFactoryFloor<GiftAidDeclaration> _GiftAidDeclarationWorker;

        public GiftAidDeclarationController(IDataFactory dataFactory)
        {
            _GiftAidDeclarationWorker = dataFactory.GetDataFactory<GiftAidDeclaration>();
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