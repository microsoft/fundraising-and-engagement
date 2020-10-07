using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class Added_Household_To_Contact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "msnfp_householdid",
                schema: "dbo",
                table: "Contact",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contact_msnfp_householdid",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_householdid");

            migrationBuilder.AddForeignKey(
                name: "FK__Contact__HouseHold__0A688BB9",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_householdid",
                principalSchema: "dbo",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Contact__HouseHold__0A688BB9",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_msnfp_householdid",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_householdid",
                schema: "dbo",
                table: "Contact");
        }
    }
}
