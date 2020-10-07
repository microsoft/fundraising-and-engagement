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
    public class ProductController : ControllerBase
    {
        private static ProductWorker _productWorker;

        public ProductController(DataFactory dataFactory)
        {
            _productWorker = (ProductWorker)dataFactory.GetDataFactory<Product>();
        }

        // GET api/Product/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var productRecord = _productWorker.GetById(id);

            string json = JsonConvert.SerializeObject(productRecord);

            return json;
        }

        // POST api/Product/CreateProduct (Body is JSON)
        [HttpPost]
        [Route("CreateProduct")]
        public HttpResponseMessage CreateProduct(Product createRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the Product record in the Azure SQL DB:
                int productResult = _productWorker.UpdateCreate(createRecord);
                if (productResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (productResult == 0)
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

        // POST api/Product/UpdateProduct (Body is JSON)
        [HttpPost]
        [Route("UpdateProduct")]
        public HttpResponseMessage UpdateProduct(Product updateRecord)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                if (updateRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Update the Product record in the Azure SQL DB:
                int productResult = _productWorker.UpdateCreate(updateRecord);
                if (productResult > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                // Existed already:
                else if (productResult == 0)
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

        // DELETE api/Product/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _productWorker.Delete(id);
            }
        }
    }
}