using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class UpdatesForDonorCommitment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                schema: "dbo",
                table: "DonorCommitment",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DonorCommitmentId",
                schema: "dbo",
                table: "DonorCommitment",
                nullable: false,
                defaultValueSql: "(newid())",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookDate",
                schema: "dbo",
                table: "DonorCommitment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Donor",
                schema: "dbo",
                table: "DonorCommitment",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountBalance",
                schema: "dbo",
                table: "DonorCommitment",
                type: "money",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookDate",
                schema: "dbo",
                table: "DonorCommitment");

            migrationBuilder.DropColumn(
                name: "Donor",
                schema: "dbo",
                table: "DonorCommitment");

            migrationBuilder.DropColumn(
                name: "TotalAmountBalance",
                schema: "dbo",
                table: "DonorCommitment");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                schema: "dbo",
                table: "DonorCommitment",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DonorCommitmentId",
                schema: "dbo",
                table: "DonorCommitment",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "(newid())");
        }
    }
}
