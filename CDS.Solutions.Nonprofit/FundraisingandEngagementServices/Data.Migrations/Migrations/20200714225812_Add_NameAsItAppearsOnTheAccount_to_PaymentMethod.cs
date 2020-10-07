using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class Add_NameAsItAppearsOnTheAccount_to_PaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameAsItAppearsOnTheAccount",
                schema: "dbo",
                table: "PaymentMethod",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "dbo",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    PaymentProcessorId = table.Column<Guid>(nullable: true),
                    PaymentMethodId = table.Column<Guid>(nullable: true),
                    EventPackageId = table.Column<Guid>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    AmountRefunded = table.Column<decimal>(type: "money", nullable: true),
                    TransactionFraudCode = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionIdentifier = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionResult = table.Column<string>(maxLength: 100, nullable: true),
                    ChequeNumber = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    PaymentType = table.Column<int>(nullable: true),
                    CcBrandCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_EventPackage_EventPackageId",
                        column: x => x.EventPackageId,
                        principalSchema: "dbo",
                        principalTable: "EventPackage",
                        principalColumn: "EventPackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "dbo",
                        principalTable: "PaymentMethod",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentProcessor_PaymentProcessorId",
                        column: x => x.PaymentProcessorId,
                        principalSchema: "dbo",
                        principalTable: "PaymentProcessor",
                        principalColumn: "PaymentProcessorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_EventPackageId",
                schema: "dbo",
                table: "Payments",
                column: "EventPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodId",
                schema: "dbo",
                table: "Payments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentProcessorId",
                schema: "dbo",
                table: "Payments",
                column: "PaymentProcessorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "NameAsItAppearsOnTheAccount",
                schema: "dbo",
                table: "PaymentMethod");
        }
    }
}
