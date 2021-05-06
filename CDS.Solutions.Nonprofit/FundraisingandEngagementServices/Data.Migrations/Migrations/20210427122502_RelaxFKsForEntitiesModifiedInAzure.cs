using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class RelaxFKsForEntitiesModifiedInAzure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Account__LastEventPackageId__0A688BB2",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK__Account__LastTransactionId__0A688BB3",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK__Account__PrimaryMembershipId__0A688BB4",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK__Contact__LastEventPackageId__0A688BB6",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK__Contact__LastTransactionId__0A688BB7",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK__Contact__PrimaryMembershipId__0A688BB8",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK__Contact__HouseHold__0A688BB9",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK__Response__Paymen__7EF6D905",
                schema: "dbo",
                table: "Response");

            migrationBuilder.DropForeignKey(
                name: "FK__Response__Regist__7FEAFD3E",
                schema: "dbo",
                table: "Response");

            migrationBuilder.DropForeignKey(
                name: "FK__Response__Transa__7E02B4CC",
                schema: "dbo",
                table: "Response");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Confi__625A9A57",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Event__65370702",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Membe__625A9A58",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Membe__625A9A59",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__trans__6166761E",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__trans__634EBE90",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK__Account__LastEventPackageId__0A688BB2",
                schema: "dbo",
                table: "Account",
                column: "msnfp_LastEventPackageId",
                principalSchema: "dbo",
                principalTable: "EventPackage",
                principalColumn: "EventPackageId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Account__LastTransactionId__0A688BB3",
                schema: "dbo",
                table: "Account",
                column: "msnfp_LastTransactionId",
                principalSchema: "dbo",
                principalTable: "Transaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Account__PrimaryMembershipId__0A688BB4",
                schema: "dbo",
                table: "Account",
                column: "msnfp_PrimaryMembershipId",
                principalSchema: "dbo",
                principalTable: "Membership",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Contact__LastEventPackageId__0A688BB6",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_LastEventPackageId",
                principalSchema: "dbo",
                principalTable: "EventPackage",
                principalColumn: "EventPackageId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Contact__LastTransactionId__0A688BB7",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_LastTransactionId",
                principalSchema: "dbo",
                principalTable: "Transaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Contact__PrimaryMembershipId__0A688BB8",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_PrimaryMembershipId",
                principalSchema: "dbo",
                principalTable: "Membership",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Contact__HouseHold__0A688BB9",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_householdid",
                principalSchema: "dbo",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Response__Paymen__7EF6D905",
                schema: "dbo",
                table: "Response",
                column: "PaymentScheduleId",
                principalSchema: "dbo",
                principalTable: "PaymentSchedule",
                principalColumn: "PaymentScheduleId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Response__Regist__7FEAFD3E",
                schema: "dbo",
                table: "Response",
                column: "RegistrationPackageId",
                principalSchema: "dbo",
                principalTable: "Registration",
                principalColumn: "RegistrationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Response__Transa__7E02B4CC",
                schema: "dbo",
                table: "Response",
                column: "TransactionId",
                principalSchema: "dbo",
                principalTable: "Transaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Confi__625A9A57",
                schema: "dbo",
                table: "Transaction",
                column: "ConfigurationId",
                principalSchema: "dbo",
                principalTable: "Configuration",
                principalColumn: "ConfigurationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Event__65370702",
                schema: "dbo",
                table: "Transaction",
                column: "EventId",
                principalSchema: "dbo",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Membe__625A9A58",
                schema: "dbo",
                table: "Transaction",
                column: "MembershipId",
                principalSchema: "dbo",
                principalTable: "MembershipCategory",
                principalColumn: "MembershipCategoryId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Membe__625A9A59",
                schema: "dbo",
                table: "Transaction",
                column: "MembershipInstanceId",
                principalSchema: "dbo",
                principalTable: "Membership",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__trans__6166761E",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionPaymentMethodId",
                principalSchema: "dbo",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__trans__634EBE90",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionPaymentScheduleId",
                principalSchema: "dbo",
                principalTable: "PaymentSchedule",
                principalColumn: "PaymentScheduleId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Account__LastEventPackageId__0A688BB2",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK__Account__LastTransactionId__0A688BB3",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK__Account__PrimaryMembershipId__0A688BB4",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK__Contact__LastEventPackageId__0A688BB6",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK__Contact__LastTransactionId__0A688BB7",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK__Contact__PrimaryMembershipId__0A688BB8",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK__Contact__HouseHold__0A688BB9",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK__Response__Paymen__7EF6D905",
                schema: "dbo",
                table: "Response");

            migrationBuilder.DropForeignKey(
                name: "FK__Response__Regist__7FEAFD3E",
                schema: "dbo",
                table: "Response");

            migrationBuilder.DropForeignKey(
                name: "FK__Response__Transa__7E02B4CC",
                schema: "dbo",
                table: "Response");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Confi__625A9A57",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Event__65370702",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Membe__625A9A58",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Membe__625A9A59",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__trans__6166761E",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__trans__634EBE90",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK__Account__LastEventPackageId__0A688BB2",
                schema: "dbo",
                table: "Account",
                column: "msnfp_LastEventPackageId",
                principalSchema: "dbo",
                principalTable: "EventPackage",
                principalColumn: "EventPackageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Account__LastTransactionId__0A688BB3",
                schema: "dbo",
                table: "Account",
                column: "msnfp_LastTransactionId",
                principalSchema: "dbo",
                principalTable: "Transaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Account__PrimaryMembershipId__0A688BB4",
                schema: "dbo",
                table: "Account",
                column: "msnfp_PrimaryMembershipId",
                principalSchema: "dbo",
                principalTable: "Membership",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Contact__LastEventPackageId__0A688BB6",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_LastEventPackageId",
                principalSchema: "dbo",
                principalTable: "EventPackage",
                principalColumn: "EventPackageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Contact__LastTransactionId__0A688BB7",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_LastTransactionId",
                principalSchema: "dbo",
                principalTable: "Transaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Contact__PrimaryMembershipId__0A688BB8",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_PrimaryMembershipId",
                principalSchema: "dbo",
                principalTable: "Membership",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Contact__HouseHold__0A688BB9",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_householdid",
                principalSchema: "dbo",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Response__Paymen__7EF6D905",
                schema: "dbo",
                table: "Response",
                column: "PaymentScheduleId",
                principalSchema: "dbo",
                principalTable: "PaymentSchedule",
                principalColumn: "PaymentScheduleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Response__Regist__7FEAFD3E",
                schema: "dbo",
                table: "Response",
                column: "RegistrationPackageId",
                principalSchema: "dbo",
                principalTable: "Registration",
                principalColumn: "RegistrationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Response__Transa__7E02B4CC",
                schema: "dbo",
                table: "Response",
                column: "TransactionId",
                principalSchema: "dbo",
                principalTable: "Transaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Confi__625A9A57",
                schema: "dbo",
                table: "Transaction",
                column: "ConfigurationId",
                principalSchema: "dbo",
                principalTable: "Configuration",
                principalColumn: "ConfigurationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Event__65370702",
                schema: "dbo",
                table: "Transaction",
                column: "EventId",
                principalSchema: "dbo",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Membe__625A9A58",
                schema: "dbo",
                table: "Transaction",
                column: "MembershipId",
                principalSchema: "dbo",
                principalTable: "MembershipCategory",
                principalColumn: "MembershipCategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Membe__625A9A59",
                schema: "dbo",
                table: "Transaction",
                column: "MembershipInstanceId",
                principalSchema: "dbo",
                principalTable: "Membership",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__trans__6166761E",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionPaymentMethodId",
                principalSchema: "dbo",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__trans__634EBE90",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionPaymentScheduleId",
                principalSchema: "dbo",
                principalTable: "PaymentSchedule",
                principalColumn: "PaymentScheduleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
