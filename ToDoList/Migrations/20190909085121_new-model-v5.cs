using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.Migrations
{
    public partial class newmodelv5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_UserId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "UsersGroup");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Groups");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UsersGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsersGroup_UserId",
                table: "UsersGroup",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroup_Users_UserId",
                table: "UsersGroup",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroup_Users_UserId",
                table: "UsersGroup");

            migrationBuilder.DropIndex(
                name: "IX_UsersGroup_UserId",
                table: "UsersGroup");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsersGroup");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "UsersGroup",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
