using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class AddDataverseSyncToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataverseSyncToken",
                schema: "dbo",
                columns: table => new
                {
                    EntityLogicalName = table.Column<string>(maxLength: 200, nullable: false),
                    TokenValue = table.Column<string>(maxLength: 100, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataverseSyncToken", x => x.EntityLogicalName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataverseSyncToken",
                schema: "dbo");
        }
    }
}