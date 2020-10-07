namespace PaymentDriver.Models
{
	public class TestDataOption
	{
		public int MaxRecords { get; set; } = 100;

		public int PaymentProcessorCode { set; get; } = 0;

		public bool UseMock { get; set; } = true;
	}
}
