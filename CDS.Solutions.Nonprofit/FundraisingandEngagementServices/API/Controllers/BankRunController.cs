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
	public class BankRunController : ControllerBase
	{
		private static BankRunWorker _BankRunWorker;

		public BankRunController(DataFactory dataFactory)
		{
			_BankRunWorker = (BankRunWorker)dataFactory.GetDataFactory<BankRun>();
		}

		// GET api/BankRun/5
		[HttpGet("{id}")]
		public ActionResult<string> Get(Guid id)
		{
			if (id == null)
			{
				return "";
			}

			var BankRunRecord = _BankRunWorker.GetById(id);

			string json = JsonConvert.SerializeObject(BankRunRecord);

			return json;
		}


		// POST api/BankRun/CreateBankRun (Body is JSON)
		[HttpPost]
		[Route("CreateBankRun")]
		public HttpResponseMessage CreateBankRun(BankRun createRecord)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

				if (createRecord == null)
				{
					return new HttpResponseMessage(HttpStatusCode.BadRequest);
				}

				// Create the BankRun record in the Azure SQL DB:
				int BankRunResult = _BankRunWorker.UpdateCreate(createRecord);
				if (BankRunResult > 0)
				{
					return new HttpResponseMessage(HttpStatusCode.OK);
				}
				// Existed already:
				else if (BankRunResult == 0)
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

		// POST api/BankRun/UpdateBankRun (Body is JSON)
		[HttpPost]
		[Route("UpdateBankRun")]
		public HttpResponseMessage UpdateBankRun(BankRun updateRecord)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

				if (updateRecord == null)
				{
					return new HttpResponseMessage(HttpStatusCode.BadRequest);
				}

				// Update the BankRun record in the Azure SQL DB:
				int BankRunResult = _BankRunWorker.UpdateCreate(updateRecord);
				if (BankRunResult > 0)
				{
					return new HttpResponseMessage(HttpStatusCode.OK);
				}
				// Existed already:
				else if (BankRunResult == 0)
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

		// DELETE api/BankRun/5
		[HttpDelete("{id}")]
		public void Delete(Guid id)
		{
			if (id != null)
			{
				_BankRunWorker.Delete(id);
			}
		}
	}
}