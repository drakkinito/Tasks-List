using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.Migrations
{
    public partial class newmodelv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Groups_GroupItemId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroup_Groups_GroupItemId",
                table: "UsersGroup");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UsersGroup",
                newName: "UserEmail");

            migrationBuilder.AlterColumn<int>(
                name: "GroupItemId",
                table: "UsersGroup",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "UsersGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "GroupItemId",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Groups_GroupItemId",
                table: "Tasks",
                column: "GroupItemId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroup_Groups_GroupItemId",
                table: "UsersGroup",
                column: "GroupItemId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Groups_GroupItemId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroup_Groups_GroupItemId",
                table: "UsersGroup");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "UsersGroup");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "UsersGroup",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "GroupItemId",
                table: "UsersGroup",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GroupItemId",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Groups_GroupItemId",
                table: "Tasks",
                column: "GroupItemId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroup_Groups_GroupItemId",
                table: "UsersGroup",
                column: "GroupItemId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
