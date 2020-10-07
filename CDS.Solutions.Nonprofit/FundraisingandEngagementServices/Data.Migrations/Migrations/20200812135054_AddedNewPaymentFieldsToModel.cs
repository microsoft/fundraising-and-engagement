using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class AddedNewPaymentFieldsToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_EventPackage_EventPackageId",
                schema: "dbo",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentProcessor_PaymentProcessorId",
                schema: "dbo",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                schema: "dbo",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CcBrandCode",
                schema: "dbo",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "Payments",
                schema: "dbo",
                newName: "Payment",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_PaymentProcessorId",
                schema: "dbo",
                table: "Payment",
                newName: "IX_Payment_PaymentProcessorId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_PaymentMethodId",
                schema: "dbo",
                table: "Payment",
                newName: "IX_Payment_PaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_EventPackageId",
                schema: "dbo",
                table: "Payment",
                newName: "IX_Payment_EventPackageId");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountBalance",
                schema: "dbo",
                table: "Payment",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CcBrandCodePayment",
                schema: "dbo",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ConfigurationId",
                schema: "dbo",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRefunded",
                schema: "dbo",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceIdentifier",
                schema: "dbo",
                table: "Payment",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ResponseId",
                schema: "dbo",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                schema: "dbo",
                table: "Payment",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_EventPackage_EventPackageId",
                schema: "dbo",
                table: "Payment",
                column: "EventPackageId",
                principalSchema: "dbo",
                principalTable: "EventPackage",
                principalColumn: "EventPackageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Payment",
                column: "PaymentMethodId",
                principalSchema: "dbo",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentProcessor_PaymentProcessorId",
                schema: "dbo",
                table: "Payment",
                column: "PaymentProcessorId",
                principalSchema: "dbo",
                principalTable: "PaymentProcessor",
                principalColumn: "PaymentProcessorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_EventPackage_EventPackageId",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentProcessor_PaymentProcessorId",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "AmountBalance",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CcBrandCodePayment",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ConfigurationId",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "DateRefunded",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "InvoiceIdentifier",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ResponseId",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                schema: "dbo",
                newName: "Payments",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_PaymentProcessorId",
                schema: "dbo",
                table: "Payments",
                newName: "IX_Payments_PaymentProcessorId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_PaymentMethodId",
                schema: "dbo",
                table: "Payments",
                newName: "IX_Payments_PaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_EventPackageId",
                schema: "dbo",
                table: "Payments",
                newName: "IX_Payments_EventPackageId");

            migrationBuilder.AddColumn<int>(
                name: "CcBrandCode",
                schema: "dbo",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                schema: "dbo",
                table: "Payments",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_EventPackage_EventPackageId",
                schema: "dbo",
                table: "Payments",
                column: "EventPackageId",
                principalSchema: "dbo",
                principalTable: "EventPackage",
                principalColumn: "EventPackageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Payments",
                column: "PaymentMethodId",
                principalSchema: "dbo",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentProcessor_PaymentProcessorId",
                schema: "dbo",
                table: "Payments",
                column: "PaymentProcessorId",
                principalSchema: "dbo",
                principalTable: "PaymentProcessor",
                principalColumn: "PaymentProcessorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
