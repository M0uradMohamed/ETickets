using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETickets.Migrations
{
    /// <inheritdoc />
    public partial class nullcolum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_purchases_purchaseId",
                table: "OrderedItems");

            migrationBuilder.AlterColumn<int>(
                name: "purchaseId",
                table: "OrderedItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_purchases_purchaseId",
                table: "OrderedItems",
                column: "purchaseId",
                principalTable: "purchases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_purchases_purchaseId",
                table: "OrderedItems");

            migrationBuilder.AlterColumn<int>(
                name: "purchaseId",
                table: "OrderedItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_purchases_purchaseId",
                table: "OrderedItems",
                column: "purchaseId",
                principalTable: "purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
