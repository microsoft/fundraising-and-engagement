using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class removeResponseRegistrationPackageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Response__Regist__7FEAFD3E",
                schema: "dbo",
                table: "Response");

            migrationBuilder.DropIndex(
                name: "IX_Response_RegistrationPackageId",
                schema: "dbo",
                table: "Response");

            migrationBuilder.DropColumn(
                name: "RegistrationPackageId",
                schema: "dbo",
                table: "Response");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegistrationPackageId",
                schema: "dbo",
                table: "Response",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Response_RegistrationPackageId",
                schema: "dbo",
                table: "Response",
                column: "RegistrationPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK__Response__Regist__7FEAFD3E",
                schema: "dbo",
                table: "Response",
                column: "RegistrationPackageId",
                principalSchema: "dbo",
                principalTable: "Registration",
                principalColumn: "RegistrationId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
