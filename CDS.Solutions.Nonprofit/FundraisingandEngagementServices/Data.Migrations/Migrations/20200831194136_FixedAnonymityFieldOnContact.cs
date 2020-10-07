using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class FixedAnonymityFieldOnContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "msnfp_Anonymity",
                schema: "dbo",
                table: "Contact",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "msnfp_Anonymity",
                schema: "dbo",
                table: "Contact",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
