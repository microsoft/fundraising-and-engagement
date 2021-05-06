using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class RemoveReplacesReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Receipt__Replace__793DFFAF",
                schema: "dbo",
                table: "Receipt");

            migrationBuilder.DropIndex(
                name: "IX_Receipt_ReplacesReceiptId",
                schema: "dbo",
                table: "Receipt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Receipt_ReplacesReceiptId",
                schema: "dbo",
                table: "Receipt",
                column: "ReplacesReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK__Receipt__Replace__793DFFAF",
                schema: "dbo",
                table: "Receipt",
                column: "ReplacesReceiptId",
                principalSchema: "dbo",
                principalTable: "Receipt",
                principalColumn: "ReceiptId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}