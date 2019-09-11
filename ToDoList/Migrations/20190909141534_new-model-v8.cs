using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.Migrations
{
    public partial class newmodelv8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Private",
                table: "Groups");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Groups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Groups");

            migrationBuilder.AddColumn<bool>(
                name: "Private",
                table: "Groups",
                nullable: false,
                defaultValue: false);
        }
    }
}
