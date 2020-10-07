using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class Removed_ReceiptTemplate_Added_EventSponsorship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptTemplate",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "EventSponsorship",
                schema: "dbo",
                columns: table => new
                {
                    EventSponsorshipId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    Advantage = table.Column<decimal>(type: "money", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Order = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    FromAmount = table.Column<decimal>(type: "money", nullable: true),
                    ValAvailable = table.Column<int>(nullable: true),
                    ValSold = table.Column<decimal>(type: "money", nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    SumSold = table.Column<decimal>(type: "money", nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSponsorship", x => x.EventSponsorshipId);
                    table.ForeignKey(
                        name: "FK__EventSpon__Event__13F1F5EB",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventSponsorship_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsorship_EventId",
                schema: "dbo",
                table: "EventSponsorship",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsorship_TransactionCurrencyId",
                schema: "dbo",
                table: "EventSponsorship",
                column: "TransactionCurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSponsorship",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "ReceiptTemplate",
                schema: "dbo",
                columns: table => new
                {
                    ReceiptTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DocxTemplate = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    EmailHtmlBodyTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailTextBodyTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FooterImage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HTMLAcknowledgement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HTMLReceipt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeaderImage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OwningBusinessUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PreferredLanguage = table.Column<int>(type: "int", nullable: true),
                    SignatureImage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StateCode = table.Column<int>(type: "int", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TemplateTypeCode = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptTemplate", x => x.ReceiptTemplateId);
                });
        }
    }
}
