using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETickets.Migrations
{
    /// <inheritdoc />
    public partial class editIT_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "purchaseId",
                table: "OrderedItems",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_purchaseId",
                table: "OrderedItems",
                column: "purchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_purchases_purchaseId",
                table: "OrderedItems",
                column: "purchaseId",
                principalTable: "purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_purchases_purchaseId",
                table: "OrderedItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedItems_purchaseId",
                table: "OrderedItems");

            migrationBuilder.DropColumn(
                name: "purchaseId",
                table: "OrderedItems");
        }
    }
}
