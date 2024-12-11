using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo_Code_First.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesDirectoryFieldForProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagesDirectory",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagesDirectory",
                table: "Products");
        }
    }
}
