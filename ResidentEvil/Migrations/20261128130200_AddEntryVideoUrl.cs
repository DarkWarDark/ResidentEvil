using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResidentEvil.Migrations
{
    /// <inheritdoc />
    public partial class AddEntryVideoUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Entries",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Entries");
        }
    }
}
