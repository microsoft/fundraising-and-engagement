using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class Added_Yearly_Giving_And_Category_To_Contact_And_Account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "msnfp_year0_category",
                schema: "dbo",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_year0_giving",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_year1_category",
                schema: "dbo",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_year1_giving",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_year2_category",
                schema: "dbo",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_year2_giving",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_year3_category",
                schema: "dbo",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_year3_giving",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_year4_category",
                schema: "dbo",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_year4_giving",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_year0_category",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_year0_giving",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_year1_category",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_year1_giving",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_year2_category",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_year2_giving",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_year3_category",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_year3_giving",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_year4_category",
                schema: "dbo",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_year4_giving",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "msnfp_year0_category",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_year0_giving",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_year1_category",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_year1_giving",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_year2_category",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_year2_giving",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_year3_category",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_year3_giving",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_year4_category",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_year4_giving",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_year0_category",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_year0_giving",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_year1_category",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_year1_giving",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_year2_category",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_year2_giving",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_year3_category",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_year3_giving",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_year4_category",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_year4_giving",
                schema: "dbo",
                table: "Account");
        }
    }
}
