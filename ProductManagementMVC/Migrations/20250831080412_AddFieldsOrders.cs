using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagementMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "Sweet",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Food",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Drink",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Drink",
                table: "Orders",
                column: "Drink");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Food",
                table: "Orders",
                column: "Food");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Sweet",
                table: "Orders",
                column: "Sweet");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_Drink",
                table: "Orders",
                column: "Drink",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_Food",
                table: "Orders",
                column: "Food",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_Sweet",
                table: "Orders",
                column: "Sweet",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_Drink",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_Food",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_Sweet",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Drink",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Food",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Sweet",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Sweet",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Food",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Drink",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
