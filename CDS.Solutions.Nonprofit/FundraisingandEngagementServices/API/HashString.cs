using Microsoft.Extensions.Options;
using FundraisingandEngagement.Utils.ConfigModels;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace FundraisingandEngagement.Services
{
	/// <summary>
	/// Provides a method for calculating the SHA256 hash of a string.
	/// </summary>
	public class HashString
	{
		public HashString(IOptions<SaltStringConfig> optionsAccessor)
		{
			Options = optionsAccessor.Value;
		}

		public HashString()
		{ }

		public string CalculateSha256Hash(string input)
		{
			using (var hasher = SHA256.Create())
			{
				var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

				var stringBuilder = new StringBuilder();
				for (int i = 0; i < hash.Length; i++)
				{
					stringBuilder.Append(hash[i].ToString("X2"));
				}

				return stringBuilder.ToString();
			}
		}

		public bool ApiKeyMatched(string input)
		{
			var hashedInput = this.CalculateSha256Hash(input);
			return String.Compare(hashedInput, Options.SaltedGatewayAPIKey, CultureInfo.InvariantCulture, CompareOptions.Ordinal) == 0;
		}

		public SaltStringConfig Options { get; } // set only via Secret Manager
	}
}
