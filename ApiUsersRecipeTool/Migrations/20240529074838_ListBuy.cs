using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiUsersRecipeTool.Migrations
{
    /// <inheritdoc />
    public partial class ListBuy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_BuyLists_BuyListId",
                table: "Ingredients");

            migrationBuilder.AlterColumn<int>(
                name: "BuyListId",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BuyLists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuyLists_UserId",
                table: "BuyLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyLists_Users_UserId",
                table: "BuyLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_BuyLists_BuyListId",
                table: "Ingredients",
                column: "BuyListId",
                principalTable: "BuyLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyLists_Users_UserId",
                table: "BuyLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_BuyLists_BuyListId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_BuyLists_UserId",
                table: "BuyLists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BuyLists");

            migrationBuilder.AlterColumn<int>(
                name: "BuyListId",
                table: "Ingredients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_BuyLists_BuyListId",
                table: "Ingredients",
                column: "BuyListId",
                principalTable: "BuyLists",
                principalColumn: "Id");
        }
    }
}
