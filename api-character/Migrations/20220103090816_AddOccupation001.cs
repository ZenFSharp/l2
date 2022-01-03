using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_character.Migrations
{
    public partial class AddOccupation001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Character",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LogoutOn",
                table: "Character",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OccupationId",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "LogoutOn",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "OccupationId",
                table: "Character");
        }
    }
}
