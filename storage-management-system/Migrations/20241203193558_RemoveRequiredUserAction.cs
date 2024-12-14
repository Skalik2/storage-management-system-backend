using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storage_management_system.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRequiredUserAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAction_Box_BoxId",
                table: "UserAction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAction_User_UserId",
                table: "UserAction");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserAction",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "BoxId",
                table: "UserAction",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAction_Box_BoxId",
                table: "UserAction",
                column: "BoxId",
                principalTable: "Box",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAction_User_UserId",
                table: "UserAction",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAction_Box_BoxId",
                table: "UserAction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAction_User_UserId",
                table: "UserAction");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserAction",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BoxId",
                table: "UserAction",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAction_Box_BoxId",
                table: "UserAction",
                column: "BoxId",
                principalTable: "Box",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAction_User_UserId",
                table: "UserAction",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
