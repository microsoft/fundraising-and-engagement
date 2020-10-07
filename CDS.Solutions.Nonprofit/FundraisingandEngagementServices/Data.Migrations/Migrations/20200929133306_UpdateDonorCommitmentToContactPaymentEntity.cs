using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class UpdateDonorCommitmentToContactPaymentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Donor",
                schema: "dbo",
                table: "DonorCommitment");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "dbo",
                table: "DonorCommitment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerIdType",
                schema: "dbo",
                table: "DonorCommitment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "dbo",
                table: "DonorCommitment");

            migrationBuilder.DropColumn(
                name: "CustomerIdType",
                schema: "dbo",
                table: "DonorCommitment");

            migrationBuilder.AddColumn<Guid>(
                name: "Donor",
                schema: "dbo",
                table: "DonorCommitment",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
