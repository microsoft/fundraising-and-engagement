using System;

namespace FundraisingandEngagement.Models
{
	public sealed class XrmException : Exception
	{
		public bool EntityDoesNotExists => Message?.EndsWith("Does Not Exist", StringComparison.OrdinalIgnoreCase) ?? false;

		public XrmException(string message)
			: base(message)
		{
		}

		public XrmException(Exception innerException) 
			: base(innerException.Message, innerException)
		{
		}
	}
}
