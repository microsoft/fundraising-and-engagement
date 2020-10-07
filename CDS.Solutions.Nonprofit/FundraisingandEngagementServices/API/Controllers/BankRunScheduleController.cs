using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using FundraisingandEngagement.DataFactory;
using FundraisingandEngagement.DataFactory.Workers;
using FundraisingandEngagement.Models.Entities;
using Newtonsoft.Json;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[EnableCors("AllowOrigin")]
	public class BankRunScheduleController : ControllerBase
	{
		private static BankRunScheduleWorker _BankRunScheduleWorker;

		public BankRunScheduleController(DataFactory dataFactory)
		{
			_BankRunScheduleWorker = (BankRunScheduleWorker)dataFactory.GetDataFactory<BankRunSchedule>();
		}

		// GET api/BankRunSchedule/5
		[HttpGet("{id}")]
		public ActionResult<string> Get(Guid id)
		{
			if (id == null)
			{
				return "";
			}

			var BankRunScheduleRecord = _BankRunScheduleWorker.GetById(id);

			string json = JsonConvert.SerializeObject(BankRunScheduleRecord);

			return json;
		}


		// POST api/BankRunSchedule/CreateBankRunSchedule (Body is JSON)
		[HttpPost]
		[Route("CreateBankRunSchedule")]
		public HttpResponseMessage CreateBankRunSchedule(BankRunSchedule createRecord)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

				if (createRecord == null)
				{
					return new HttpResponseMessage(HttpStatusCode.BadRequest);
				}

				// Create the BankRunSchedule record in the Azure SQL DB:
				int BankRunScheduleResult = _BankRunScheduleWorker.UpdateCreate(createRecord);
				if (BankRunScheduleResult > 0)
				{
					return new HttpResponseMessage(HttpStatusCode.OK);
				}
				// Existed already:
				else if (BankRunScheduleResult == 0)
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

		// POST api/BankRunSchedule/UpdateBankRunSchedule (Body is JSON)
		[HttpPost]
		[Route("UpdateBankRunSchedule")]
		public HttpResponseMessage UpdateBankRunSchedule(BankRunSchedule updateRecord)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

				if (updateRecord == null)
				{
					return new HttpResponseMessage(HttpStatusCode.BadRequest);
				}

				// Update the BankRunSchedule record in the Azure SQL DB:
				int BankRunScheduleResult = _BankRunScheduleWorker.UpdateCreate(updateRecord);
				if (BankRunScheduleResult > 0)
				{
					return new HttpResponseMessage(HttpStatusCode.OK);
				}
				// Existed already:
				else if (BankRunScheduleResult == 0)
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

		// DELETE api/BankRunSchedule/5
		[HttpDelete("{id}")]
		public void Delete(Guid id)
		{
			if (id != null)
			{
				_BankRunScheduleWorker.Delete(id);
			}
		}
	}
}