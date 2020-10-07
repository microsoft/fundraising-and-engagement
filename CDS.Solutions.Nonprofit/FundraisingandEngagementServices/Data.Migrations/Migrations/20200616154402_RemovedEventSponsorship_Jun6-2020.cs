using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class RemovedEventSponsorship_Jun62020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSponsorship",
                schema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventSponsorship",
                schema: "dbo",
                columns: table => new
                {
                    EventSponsorshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Advantage = table.Column<decimal>(type: "money", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FromAmount = table.Column<decimal>(type: "money", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    StateCode = table.Column<int>(type: "int", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    SumSold = table.Column<int>(type: "int", nullable: true),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ValAvailable = table.Column<int>(type: "int", nullable: true),
                    ValSold = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSponsorship", x => x.EventSponsorshipId);
                    table.ForeignKey(
                        name: "FK_EventSponsorship_Event_EventId",
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
    }
}
