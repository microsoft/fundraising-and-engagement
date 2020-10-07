using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class AddedLifeTimeGivingSumFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_lifetimegivingsum",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_lifetimegivingsum",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "msnfp_lifetimegivingsum",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_lifetimegivingsum",
                schema: "dbo",
                table: "Account");
        }
    }
}
