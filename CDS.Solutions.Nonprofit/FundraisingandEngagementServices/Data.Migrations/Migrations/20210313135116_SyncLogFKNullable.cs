using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class SyncLogFKNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Event__TermsOfRe__01D345B0",
                schema: "dbo",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_SyncException_Transaction_TransactionId",
                schema: "dbo",
                table: "SyncException");

            migrationBuilder.DropTable(
                name: "TermsOfReference",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Event_TermsOfReferenceId",
                schema: "dbo",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "TermsOfReferenceId",
                schema: "dbo",
                table: "Event");

            migrationBuilder.AddForeignKey(
                name: "FK_SyncException_Transaction_TransactionId",
                schema: "dbo",
                table: "SyncException",
                column: "TransactionId",
                principalSchema: "dbo",
                principalTable: "Transaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SyncException_Transaction_TransactionId",
                schema: "dbo",
                table: "SyncException");

            migrationBuilder.AddColumn<Guid>(
                name: "TermsOfReferenceId",
                schema: "dbo",
                table: "Event",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TermsOfReference",
                schema: "dbo",
                columns: table => new
                {
                    TermsOfReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CcvMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverCostsMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailureMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Footer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiftAidAcceptence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiftAidDeclaration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiftAidDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivacyPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivacyUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowPrivacy = table.Column<bool>(type: "bit", nullable: true),
                    ShowTermsConditions = table.Column<bool>(type: "bit", nullable: true),
                    Signup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateCode = table.Column<int>(type: "int", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermsConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsConditionsUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsOfReference", x => x.TermsOfReferenceId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_TermsOfReferenceId",
                schema: "dbo",
                table: "Event",
                column: "TermsOfReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK__Event__TermsOfRe__01D345B0",
                schema: "dbo",
                table: "Event",
                column: "TermsOfReferenceId",
                principalSchema: "dbo",
                principalTable: "TermsOfReference",
                principalColumn: "TermsOfReferenceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SyncException_Transaction_TransactionId",
                schema: "dbo",
                table: "SyncException",
                column: "TransactionId",
                principalSchema: "dbo",
                principalTable: "Transaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}