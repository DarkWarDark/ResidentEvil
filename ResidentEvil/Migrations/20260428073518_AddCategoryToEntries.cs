using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResidentEvil.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Entries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Entries");
        }
    }
}
