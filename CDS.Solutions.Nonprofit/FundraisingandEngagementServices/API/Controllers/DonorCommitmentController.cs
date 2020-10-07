using System;
using System.Net;
using System.Net.Http;
using FundraisingandEngagement.DataFactory;
using FundraisingandEngagement.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DonorCommitmentController : ControllerBase
	{
		private static DonorCommitmentWorker _donorCommitmentWorker;

		public DonorCommitmentController(DataFactory dataFactory)
		{
			_donorCommitmentWorker = (DonorCommitmentWorker)dataFactory.GetDataFactory<DonorCommitment>();
		}

		// GET api/DonorCommitment/5
		[HttpGet("{id}")]
		public ActionResult<string> Get(Guid id)
		{
			if (id == null)
			{
				return "";
			}

			var DesignationRecord = _donorCommitmentWorker.GetById(id);

			string json = JsonConvert.SerializeObject(DesignationRecord);

			return json;
		}

		// POST api/DonorCommitment/CreateDonorCommitment (Body is JSON)
		[HttpPost]
		[Route("CreateDonorCommitment")]
		public HttpResponseMessage CreateDonorCommitment(DonorCommitment createRecord)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

				if (createRecord == null)
				{
					return new HttpResponseMessage(HttpStatusCode.BadRequest);
				}

				// Create the DonorCommitment record in the Azure SQL DB:
				int DonorCommitment = _donorCommitmentWorker.UpdateCreate(createRecord);
				if (DonorCommitment > 0)
				{
					return new HttpResponseMessage(HttpStatusCode.OK);
				}
				// Existed already:
				else if (DonorCommitment == 0)
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

		// POST api/Designation/UpdateDonorCommitment (Body is JSON)
		[HttpPost]
		[Route("UpdateDonorCommitment")]
		public HttpResponseMessage UpdateDonorCommitment(DonorCommitment updateRecord)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

				if (updateRecord == null)
				{
					return new HttpResponseMessage(HttpStatusCode.BadRequest);
				}

				// Update the Designation record in the Azure SQL DB:
				int DonorCommitment = _donorCommitmentWorker.UpdateCreate(updateRecord);
				if (DonorCommitment > 0)
				{
					return new HttpResponseMessage(HttpStatusCode.OK);
				}
				// Existed already:
				else if (DonorCommitment == 0)
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


		// DELETE api/Designation/5
		[HttpDelete("{id}")]
		public void Delete(Guid id)
		{
			if (id != null)
			{
				_donorCommitmentWorker.Delete(id);
			}
		}
	}
}
