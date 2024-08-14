using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microserve.Services.LocatorAPI.Migrations
{
    /// <inheritdoc />
    public partial class addExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Business_status",
                table: "Locators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone_number",
                table: "Locators",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Business_status",
                table: "Locators");

            migrationBuilder.DropColumn(
                name: "Phone_number",
                table: "Locators");
        }
    }
}
