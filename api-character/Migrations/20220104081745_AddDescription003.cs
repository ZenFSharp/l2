using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_character.Migrations
{
    public partial class AddDescription003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Race",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Occupation",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Race");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Occupation");
        }
    }
}
