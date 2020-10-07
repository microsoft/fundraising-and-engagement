using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class Proper_PascalCasing_forProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "registrationid",
                schema: "dbo",
                table: "RegistrationPreference",
                newName: "RegistrationId");

            migrationBuilder.RenameColumn(
                name: "other",
                schema: "dbo",
                table: "RegistrationPreference",
                newName: "Other");

            migrationBuilder.RenameColumn(
                name: "eventid",
                schema: "dbo",
                table: "RegistrationPreference",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "registrationpreferenceid",
                schema: "dbo",
                table: "RegistrationPreference",
                newName: "RegistrationPreferenceId");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "dbo",
                table: "PreferenceCategory",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "categorycode",
                schema: "dbo",
                table: "PreferenceCategory",
                newName: "CategoryCode");

            migrationBuilder.RenameColumn(
                name: "tablenumber",
                schema: "dbo",
                table: "EventTable",
                newName: "TableNumber");

            migrationBuilder.RenameColumn(
                name: "tablecapacity",
                schema: "dbo",
                table: "EventTable",
                newName: "TableCapacity");

            migrationBuilder.RenameColumn(
                name: "identifier",
                schema: "dbo",
                table: "EventTable",
                newName: "Identifier");

            migrationBuilder.RenameColumn(
                name: "eventticketid",
                schema: "dbo",
                table: "EventTable",
                newName: "EventTicketId");

            migrationBuilder.RenameColumn(
                name: "eventid",
                schema: "dbo",
                table: "EventTable",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "eventtableid",
                schema: "dbo",
                table: "EventTable",
                newName: "EventTableId");

            migrationBuilder.RenameColumn(
                name: "preferenceid",
                schema: "dbo",
                table: "EventPreference",
                newName: "PreferenceId");

            migrationBuilder.RenameColumn(
                name: "preferencecategoryid",
                schema: "dbo",
                table: "EventPreference",
                newName: "PreferenceCategoryId");

            migrationBuilder.RenameColumn(
                name: "identifier",
                schema: "dbo",
                table: "EventPreference",
                newName: "Identifier");

            migrationBuilder.RenameColumn(
                name: "eventid",
                schema: "dbo",
                table: "EventPreference",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "eventpreferenceid",
                schema: "dbo",
                table: "EventPreference",
                newName: "EventPreferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_EventPreference_preferencecategoryid",
                schema: "dbo",
                table: "EventPreference",
                newName: "IX_EventPreference_PreferenceCategoryId");

            migrationBuilder.AddColumn<int>(
                name: "StateCode",
                schema: "dbo",
                table: "Note",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                schema: "dbo",
                table: "Note",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateCode",
                schema: "dbo",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                schema: "dbo",
                table: "Note");

            migrationBuilder.RenameColumn(
                name: "RegistrationId",
                schema: "dbo",
                table: "RegistrationPreference",
                newName: "registrationid");

            migrationBuilder.RenameColumn(
                name: "Other",
                schema: "dbo",
                table: "RegistrationPreference",
                newName: "other");

            migrationBuilder.RenameColumn(
                name: "EventId",
                schema: "dbo",
                table: "RegistrationPreference",
                newName: "eventid");

            migrationBuilder.RenameColumn(
                name: "RegistrationPreferenceId",
                schema: "dbo",
                table: "RegistrationPreference",
                newName: "registrationpreferenceid");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "dbo",
                table: "PreferenceCategory",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "CategoryCode",
                schema: "dbo",
                table: "PreferenceCategory",
                newName: "categorycode");

            migrationBuilder.RenameColumn(
                name: "TableNumber",
                schema: "dbo",
                table: "EventTable",
                newName: "tablenumber");

            migrationBuilder.RenameColumn(
                name: "TableCapacity",
                schema: "dbo",
                table: "EventTable",
                newName: "tablecapacity");

            migrationBuilder.RenameColumn(
                name: "Identifier",
                schema: "dbo",
                table: "EventTable",
                newName: "identifier");

            migrationBuilder.RenameColumn(
                name: "EventTicketId",
                schema: "dbo",
                table: "EventTable",
                newName: "eventticketid");

            migrationBuilder.RenameColumn(
                name: "EventId",
                schema: "dbo",
                table: "EventTable",
                newName: "eventid");

            migrationBuilder.RenameColumn(
                name: "EventTableId",
                schema: "dbo",
                table: "EventTable",
                newName: "eventtableid");

            migrationBuilder.RenameColumn(
                name: "PreferenceId",
                schema: "dbo",
                table: "EventPreference",
                newName: "preferenceid");

            migrationBuilder.RenameColumn(
                name: "PreferenceCategoryId",
                schema: "dbo",
                table: "EventPreference",
                newName: "preferencecategoryid");

            migrationBuilder.RenameColumn(
                name: "Identifier",
                schema: "dbo",
                table: "EventPreference",
                newName: "identifier");

            migrationBuilder.RenameColumn(
                name: "EventId",
                schema: "dbo",
                table: "EventPreference",
                newName: "eventid");

            migrationBuilder.RenameColumn(
                name: "EventPreferenceId",
                schema: "dbo",
                table: "EventPreference",
                newName: "eventpreferenceid");

            migrationBuilder.RenameIndex(
                name: "IX_EventPreference_PreferenceCategoryId",
                schema: "dbo",
                table: "EventPreference",
                newName: "IX_EventPreference_preferencecategoryid");
        }
    }
}
