using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Groups");

            migrationBuilder.AddColumn<int>(
                name: "AdminUserId",
                table: "Groups",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "Groups",
                nullable: true);
        }
    }
}
