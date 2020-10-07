using System;
using System.IO;

namespace PaymentDriver.Utils
{
	internal static class FileUtils
	{
		internal static string ReadFile(string location)
		{
			if (!File.Exists(location))
			{
				return String.Empty;
			}

			return File.ReadAllText(location);
		}
	}
}
