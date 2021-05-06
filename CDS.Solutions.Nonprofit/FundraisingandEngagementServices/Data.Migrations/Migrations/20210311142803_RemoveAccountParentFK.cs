using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class RemoveAccountParentFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Account__ParentAccountId__0A688BB5",
                schema: "dbo",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_ParentAccountId",
                schema: "dbo",
                table: "Account");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Account_ParentAccountId",
                schema: "dbo",
                table: "Account",
                column: "ParentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK__Account__ParentAccountId__0A688BB5",
                schema: "dbo",
                table: "Account",
                column: "ParentAccountId",
                principalSchema: "dbo",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}