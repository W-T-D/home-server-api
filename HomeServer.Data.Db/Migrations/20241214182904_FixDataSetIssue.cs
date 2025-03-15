using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixDataSetIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileInfo",
                table: "FileInfo");

            migrationBuilder.RenameTable(
                name: "FileInfo",
                newName: "FileInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileInfos",
                table: "FileInfos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileInfos",
                table: "FileInfos");

            migrationBuilder.RenameTable(
                name: "FileInfos",
                newName: "FileInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileInfo",
                table: "FileInfo",
                column: "Id");
        }
    }
}
