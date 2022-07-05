using Microsoft.EntityFrameworkCore.Migrations;

namespace WebServiceTask.Migrations
{
    public partial class UniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AddressLine",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Personal_FirstName",
                table: "Personal",
                column: "FirstName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personal_LastName",
                table: "Personal",
                column: "LastName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AddressLine",
                table: "Addresses",
                column: "AddressLine",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_City",
                table: "Addresses",
                column: "City",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Personal_FirstName",
                table: "Personal");

            migrationBuilder.DropIndex(
                name: "IX_Personal_LastName",
                table: "Personal");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_AddressLine",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_City",
                table: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
