using System;

namespace PaymentProcessors.Worldpay
{
	internal class WorldpayException : Exception
	{
		/// <summary>
		/// Details of the API error
		/// </summary>
		public ApiError apiError { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public WorldpayException(string message) : this(null, message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public WorldpayException(ApiError error, string message) : base(message)
		{
			apiError = error;
		}
	}

	internal class ApiError
	{
		public int httpStatusCode { get; set; }

		public string customCode { get; set; }

		public string message { get; set; }

		public string description { get; set; }

		public string errorHelpUrl { get; set; }

		public string originalRequest { get; set; }
	}
}
