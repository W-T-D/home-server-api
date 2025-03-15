using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "FileInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "FileInfo",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
