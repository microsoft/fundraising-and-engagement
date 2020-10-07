using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class UpadtedSponsoshipAndEventSponsorshipForEventReceipting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Sponsorsh__Event__17C286CF",
                schema: "dbo",
                table: "Sponsorship");

            migrationBuilder.AddColumn<Guid>(
                name: "EventSponsorshipId",
                schema: "dbo",
                table: "Sponsorship",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountNonReceiptable",
                schema: "dbo",
                table: "EventSponsorship",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountReceipted",
                schema: "dbo",
                table: "EventSponsorship",
                type: "money",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorship_EventSponsorshipId",
                schema: "dbo",
                table: "Sponsorship",
                column: "EventSponsorshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsorship_EventSponsor_EventSponsorId",
                schema: "dbo",
                table: "Sponsorship",
                column: "EventSponsorId",
                principalSchema: "dbo",
                principalTable: "EventSponsor",
                principalColumn: "EventSponsorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Sponsorsh__Event__17C286CF",
                schema: "dbo",
                table: "Sponsorship",
                column: "EventSponsorshipId",
                principalSchema: "dbo",
                principalTable: "EventSponsorship",
                principalColumn: "EventSponsorshipId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sponsorship_EventSponsor_EventSponsorId",
                schema: "dbo",
                table: "Sponsorship");

            migrationBuilder.DropForeignKey(
                name: "FK__Sponsorsh__Event__17C286CF",
                schema: "dbo",
                table: "Sponsorship");

            migrationBuilder.DropIndex(
                name: "IX_Sponsorship_EventSponsorshipId",
                schema: "dbo",
                table: "Sponsorship");

            migrationBuilder.DropColumn(
                name: "EventSponsorshipId",
                schema: "dbo",
                table: "Sponsorship");

            migrationBuilder.DropColumn(
                name: "AmountNonReceiptable",
                schema: "dbo",
                table: "EventSponsorship");

            migrationBuilder.DropColumn(
                name: "AmountReceipted",
                schema: "dbo",
                table: "EventSponsorship");

            migrationBuilder.AddForeignKey(
                name: "FK__Sponsorsh__Event__17C286CF",
                schema: "dbo",
                table: "Sponsorship",
                column: "EventSponsorId",
                principalSchema: "dbo",
                principalTable: "EventSponsor",
                principalColumn: "EventSponsorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
