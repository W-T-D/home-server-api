using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Size",
                table: "FileInfos",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "FileInfos");
        }
    }
}
