using FundraisingandEngagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FundraisingandEngagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // This controller allows anonymous requests!
    public class VersionController : ControllerBase
    {
        public ActionResult<string> Get()
        {
            return "v30.12.2020.1";
        }

        [HttpPost]
        public ActionResult<string> Hash(Input input)
        {
            if (input == null || input.V == null)
            {
                this.Response.StatusCode = 400;
                return Content("input cannot be null");
            }

            var hasher = new HashString();
            return hasher.CalculateSha256Hash(input.V);
        }

        public class Input
        {
            public string V { get; set; }
        }
    }
}
