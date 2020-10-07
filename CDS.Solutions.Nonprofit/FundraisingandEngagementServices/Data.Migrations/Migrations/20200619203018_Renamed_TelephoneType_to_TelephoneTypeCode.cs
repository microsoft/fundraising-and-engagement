using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class Renamed_TelephoneType_to_TelephoneTypeCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "msnfp_Telephone1Type",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_Telephone2Type",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_Telephone3Type",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.AddColumn<int>(
                name: "msnfp_telephone1typecode",
                schema: "dbo",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_telephone2typecode",
                schema: "dbo",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_telephone3typecode",
                schema: "dbo",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_classificationcode",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "msnfp_issolicitor",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "msnfp_signup",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "msnfp_solicitcode",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "msnfp_solicitdate",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "msnfp_vendorid",
                schema: "dbo",
                table: "Account",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "msnfp_telephone1typecode",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_telephone2typecode",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_telephone3typecode",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_classificationcode",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_issolicitor",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_signup",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_solicitcode",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_solicitdate",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_vendorid",
                schema: "dbo",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "msnfp_Telephone1Type",
                schema: "dbo",
                table: "Contact",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_Telephone2Type",
                schema: "dbo",
                table: "Contact",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_Telephone3Type",
                schema: "dbo",
                table: "Contact",
                type: "int",
                nullable: true);
        }
    }
}
