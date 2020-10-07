using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class AddingMissingColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                schema: "dbo",
                table: "SyncException");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                schema: "dbo",
                table: "SyncException");

            migrationBuilder.DropColumn(
                name: "ExecutionId",
                schema: "dbo",
                table: "SyncException");

            migrationBuilder.DropColumn(
                name: "StateCode",
                schema: "dbo",
                table: "SyncException");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                schema: "dbo",
                table: "SyncException");

            migrationBuilder.DropColumn(
                name: "SyncDate",
                schema: "dbo",
                table: "SyncException");

            migrationBuilder.AddColumn<string>(
                name: "StackTrace",
                schema: "dbo",
                table: "SyncException",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldSyncResponse",
                schema: "dbo",
                table: "Configuration",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionCurrencyId",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionCurrency_TransactionCurrencyId",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionCurrencyId",
                principalSchema: "dbo",
                principalTable: "TransactionCurrency",
                principalColumn: "TransactionCurrencyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_TransactionCurrency_TransactionCurrencyId",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_TransactionCurrencyId",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "StackTrace",
                schema: "dbo",
                table: "SyncException");

            migrationBuilder.DropColumn(
                name: "ShouldSyncResponse",
                schema: "dbo",
                table: "Configuration");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                schema: "dbo",
                table: "SyncException",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                schema: "dbo",
                table: "SyncException",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExecutionId",
                schema: "dbo",
                table: "SyncException",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateCode",
                schema: "dbo",
                table: "SyncException",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                schema: "dbo",
                table: "SyncException",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SyncDate",
                schema: "dbo",
                table: "SyncException",
                type: "datetime2",
                nullable: true);
        }
    }
}
