using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class UpdateMonerisApiKeyLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBase",
                schema: "dbo",
                table: "TransactionCurrency");

            migrationBuilder.DropColumn(
                name: "CcExpDate",
                schema: "dbo",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "ShouldSyncResponse",
                schema: "dbo",
                table: "Configuration");

            migrationBuilder.AlterColumn<string>(
                name: "MonerisApiKey",
                schema: "dbo",
                table: "PaymentProcessor",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBase",
                schema: "dbo",
                table: "TransactionCurrency",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MonerisApiKey",
                schema: "dbo",
                table: "PaymentProcessor",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CcExpDate",
                schema: "dbo",
                table: "PaymentMethod",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldSyncResponse",
                schema: "dbo",
                table: "Configuration",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}