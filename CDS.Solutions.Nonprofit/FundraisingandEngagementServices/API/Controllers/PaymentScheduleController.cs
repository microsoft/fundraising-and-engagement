using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.DataFactory;
using FundraisingandEngagement.DataFactory.Workers;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using FundraisingandEngagement.Data;
using FundraisingandEngagement;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentScheduleController : ControllerBase    
    {
        private readonly PaymentContext context;
        private readonly PaymentScheduleWorker _paymentScheduleWorker;
        private readonly ILogger<PaymentScheduleController> logger;

        public PaymentScheduleController(DataFactory dataFactory, PaymentContext context, ILogger<PaymentScheduleController> logger)
        {
            _paymentScheduleWorker = (PaymentScheduleWorker)dataFactory.GetDataFactory<PaymentSchedule>();
            this.context = context;
            this.logger = logger;
        }


        // GET api/PaymentSchedule/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            if (id == null)
            {
                return "";
            }

            var retrievedRecord = _paymentScheduleWorker.GetById(id);

            string json = JsonConvert.SerializeObject(retrievedRecord);

            return json;
        }



        // POST api/PaymentSchedule/CreatePaymentSchedule (Body is JSON)
        [HttpPost]
        [Route("CreatePaymentSchedule")]
        public HttpResponseMessage CreatePaymentSchedule(PaymentSchedule createRecord)
        {
            try
            {
                if (createRecord == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Create the entity record in the Azure SQL DB:
                int createResult = _paymentScheduleWorker.UpdateCreate(createRecord);
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


        // POST api/PaymentSchedule/UpdatePaymentSchedule (Body is JSON)
        [HttpPost]
        [ValidateModel]
        [Route("UpdatePaymentSchedule")]
        public async Task UpdatePaymentSchedule(PaymentSchedule updatedRecord)
        {
            this.logger.LogInformation($"{nameof(UpdatePaymentSchedule)}(PaymentScheduleId : {updatedRecord.PaymentScheduleId}, NextPaymentDate : {updatedRecord.NextPaymentDate}, LastPaymentDate : {updatedRecord.LastPaymentDate})");

            var query = from paymentSchedule in this.context.PaymentSchedule
                        where paymentSchedule.PaymentScheduleId == updatedRecord.PaymentScheduleId
                        select new
						{
							paymentSchedule.NextPaymentDate,
							paymentSchedule.LastPaymentDate,
						};

            var existingRecords = await query.SingleOrDefaultAsync();

			if (existingRecords != null)
			{
				var existingNextPaymentDate = existingRecords.NextPaymentDate;
				var existingLastPaymentDate = existingRecords.LastPaymentDate;

				if (updatedRecord.NextPaymentDate < existingNextPaymentDate || (updatedRecord.NextPaymentDate == null && existingNextPaymentDate != null))
				{
					this.logger.LogInformation($"Overwrting NextPaymentDate value '{updatedRecord.NextPaymentDate}' with '{existingNextPaymentDate}'");

					updatedRecord.NextPaymentDate = existingNextPaymentDate;
				}

				if (updatedRecord.LastPaymentDate < existingLastPaymentDate || (updatedRecord.LastPaymentDate == null && existingLastPaymentDate != null))
				{
					this.logger.LogInformation($"Overwrting LastPaymentDate value '{updatedRecord.LastPaymentDate}' with '{existingLastPaymentDate}'");

					updatedRecord.LastPaymentDate = existingLastPaymentDate;
				}
			}

			// Create the entity record in the Azure SQL DB:
			_paymentScheduleWorker.UpdateCreate(updatedRecord);
        }


        // DELETE api/PaymentSchedule/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            if (id != null)
            {
                _paymentScheduleWorker.Delete(id);
            }
        }
    }
}
