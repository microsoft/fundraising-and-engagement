using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FundraisingandEngagement.Services;

namespace API
{
	public sealed class PadlockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		private readonly SaltString saltString;

		public PadlockAuthenticationHandler(SaltString saltString, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
			this.saltString = saltString;
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			var key = Request.Headers["Padlock"];

			if (String.IsNullOrEmpty(key))
			{
				Logger.LogWarning("Incoming HTTP request is without Padlock key");
				return Task.FromResult(AuthenticateResult.Fail("Padlock key is null"));
			}

			if (!this.saltString.ApiKeyMatched(key))
			{
				Logger.LogWarning($"Invalid Padlock key '{key}'");
				return Task.FromResult(AuthenticateResult.Fail("Padlock key is invalid"));
			}

			var claims = new[] { new Claim("padlock", key) };
			var identity = new ClaimsIdentity(claims, Scheme.Name);
			var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
			return Task.FromResult(AuthenticateResult.Success(ticket));
		}
	}
}
