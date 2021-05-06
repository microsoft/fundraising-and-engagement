using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class RemoveParentCustomerIdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentCustomerIdType",
                schema: "dbo",
                table: "Contact");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCustomerIdType",
                schema: "dbo",
                table: "Contact",
                type: "int",
                nullable: true);
        }
    }
}