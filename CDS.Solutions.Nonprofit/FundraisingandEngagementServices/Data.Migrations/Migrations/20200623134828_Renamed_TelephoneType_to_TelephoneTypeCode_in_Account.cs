using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class Renamed_TelephoneType_to_TelephoneTypeCode_in_Account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "msnfp_Telephone1Type",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_Telephone2Type",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_Telephone3Type",
                schema: "dbo",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "msnfp_Telephone1TypeCode",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_Telephone2TypeCode",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_Telephone3TypeCode",
                schema: "dbo",
                table: "Account",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "msnfp_Telephone1TypeCode",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_Telephone2TypeCode",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_Telephone3TypeCode",
                schema: "dbo",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "msnfp_Telephone1Type",
                schema: "dbo",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_Telephone2Type",
                schema: "dbo",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_Telephone3Type",
                schema: "dbo",
                table: "Account",
                type: "int",
                nullable: true);
        }
    }
}
