using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class UpdatesToAnonyousAnonymityFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anonymity",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "msnfp_Anonymous",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_Anonymous",
                schema: "dbo",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "Anonymous",
                schema: "dbo",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "msnfp_Anonymity",
                schema: "dbo",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_Anonymity",
                schema: "dbo",
                table: "Account",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anonymous",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "msnfp_Anonymity",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_Anonymity",
                schema: "dbo",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "Anonymity",
                schema: "dbo",
                table: "Transaction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "msnfp_Anonymous",
                schema: "dbo",
                table: "Contact",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "msnfp_Anonymous",
                schema: "dbo",
                table: "Account",
                type: "bit",
                nullable: true);
        }
    }
}
