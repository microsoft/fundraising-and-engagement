using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
	public partial class Update_MissionSpecificField : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "MissioninvoiceIdentifier",
				schema: "dbo",
				table: "EventPackage");

			migrationBuilder.DropColumn(
				name: "MissionInvoiceIdentifier",
				schema: "dbo",
				table: "PaymentSchedule");

			migrationBuilder.DropColumn(
				name: "MissionInvoiceIdentifier",
				schema: "dbo",
				table: "Transaction");

			//InvoiceIdentifier
			migrationBuilder.AddColumn<string>(
				name: "InvoiceIdentifier",
				schema: "dbo",
				table: "EventPackage",
				nullable: true,
				maxLength: 100);

			migrationBuilder.AddColumn<string>(
				name: "InvoiceIdentifier",
				schema: "dbo",
				table: "PaymentSchedule",
				nullable: true,
				maxLength: 100);

			migrationBuilder.AddColumn<string>(
				name: "InvoiceIdentifier",
				schema: "dbo",
				table: "Transaction",
				nullable: true,
				maxLength: 100);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
