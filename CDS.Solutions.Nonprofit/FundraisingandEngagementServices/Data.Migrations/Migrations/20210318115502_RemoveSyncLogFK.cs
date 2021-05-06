using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class RemoveSyncLogFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SyncException_Transaction_TransactionId",
                schema: "dbo",
                table: "SyncException");

            migrationBuilder.DropIndex(
                name: "IX_SyncException_TransactionId",
                schema: "dbo",
                table: "SyncException");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SyncException_TransactionId",
                schema: "dbo",
                table: "SyncException",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SyncException_Transaction_TransactionId",
                schema: "dbo",
                table: "SyncException",
                column: "TransactionId",
                principalSchema: "dbo",
                principalTable: "Transaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}