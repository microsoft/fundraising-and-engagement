using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class Updated_To_NullableGuids_In_EventPreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__EventPreference__PreferenceCategory__18B6AB52",
                schema: "dbo",
                table: "EventPreference");

            migrationBuilder.AlterColumn<Guid>(
                name: "preferenceid",
                schema: "dbo",
                table: "EventPreference",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "preferencecategoryid",
                schema: "dbo",
                table: "EventPreference",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "eventid",
                schema: "dbo",
                table: "EventPreference",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK__EventPreference__PreferenceCategory__18B6AB52",
                schema: "dbo",
                table: "EventPreference",
                column: "preferencecategoryid",
                principalSchema: "dbo",
                principalTable: "PreferenceCategory",
                principalColumn: "preferencecategoryid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__EventPreference__PreferenceCategory__18B6AB52",
                schema: "dbo",
                table: "EventPreference");

            migrationBuilder.AlterColumn<Guid>(
                name: "preferenceid",
                schema: "dbo",
                table: "EventPreference",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "preferencecategoryid",
                schema: "dbo",
                table: "EventPreference",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "eventid",
                schema: "dbo",
                table: "EventPreference",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__EventPreference__PreferenceCategory__18B6AB52",
                schema: "dbo",
                table: "EventPreference",
                column: "preferencecategoryid",
                principalSchema: "dbo",
                principalTable: "PreferenceCategory",
                principalColumn: "preferencecategoryid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
