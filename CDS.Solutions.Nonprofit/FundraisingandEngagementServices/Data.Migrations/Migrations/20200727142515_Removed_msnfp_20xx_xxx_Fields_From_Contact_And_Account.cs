using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class Removed_msnfp_20xx_xxx_Fields_From_Contact_And_Account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "msnfp_2017ClassificationCode",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2017TransactionsReceipted",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2017TransactionsTotal",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2018ClassificationCode",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2018TransactionsReceipted",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2018TransactionsTotal",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2019ClassificationCode",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2019TransactionsReceipted",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2019TransactionsTotal",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2020ClassificationCode",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2020TransactionsReceipted",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2020TransactionsTotal",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2021ClassificationCode",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2021TransactionsReceipted",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2021TransactionsTotal",
                schema: "dbo",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "msnfp_2017ClassificationCode",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2017TransactionsReceipted",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2017TransactionsTotal",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2018ClassificationCode",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2018TransactionsReceipted",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2018TransactionsTotal",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2019ClassificationCode",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2019TransactionsReceipted",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2019TransactionsTotal",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2020ClassificationCode",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2020TransactionsReceipted",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2020TransactionsTotal",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2021ClassificationCode",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2021TransactionsReceipted",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "msnfp_2021TransactionsTotal",
                schema: "dbo",
                table: "Account");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "msnfp_2017ClassificationCode",
                schema: "dbo",
                table: "Contact",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2017TransactionsReceipted",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2017TransactionsTotal",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_2018ClassificationCode",
                schema: "dbo",
                table: "Contact",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2018TransactionsReceipted",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2018TransactionsTotal",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_2019ClassificationCode",
                schema: "dbo",
                table: "Contact",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2019TransactionsReceipted",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2019TransactionsTotal",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_2020ClassificationCode",
                schema: "dbo",
                table: "Contact",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2020TransactionsReceipted",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2020TransactionsTotal",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_2021ClassificationCode",
                schema: "dbo",
                table: "Contact",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2021TransactionsReceipted",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2021TransactionsTotal",
                schema: "dbo",
                table: "Contact",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_2017ClassificationCode",
                schema: "dbo",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2017TransactionsReceipted",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2017TransactionsTotal",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_2018ClassificationCode",
                schema: "dbo",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2018TransactionsReceipted",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2018TransactionsTotal",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_2019ClassificationCode",
                schema: "dbo",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2019TransactionsReceipted",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2019TransactionsTotal",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_2020ClassificationCode",
                schema: "dbo",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2020TransactionsReceipted",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2020TransactionsTotal",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "msnfp_2021ClassificationCode",
                schema: "dbo",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2021TransactionsReceipted",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "msnfp_2021TransactionsTotal",
                schema: "dbo",
                table: "Account",
                type: "money",
                nullable: true);
        }
    }
}
