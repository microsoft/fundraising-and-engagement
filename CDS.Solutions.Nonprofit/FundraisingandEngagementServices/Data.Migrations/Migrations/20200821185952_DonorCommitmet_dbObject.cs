using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class DonorCommitmet_dbObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonorCommitment",
                schema: "dbo",
                columns: table => new
                {
                    DonorCommitmentId = table.Column<Guid>(nullable: false),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    TotalAmount = table.Column<decimal>(nullable: true),
                    AppealId = table.Column<Guid>(nullable: true),
                    PackageId = table.Column<Guid>(nullable: true),
                    CampaignId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorCommitment", x => x.DonorCommitmentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonorCommitment",
                schema: "dbo");
        }
    }
}
