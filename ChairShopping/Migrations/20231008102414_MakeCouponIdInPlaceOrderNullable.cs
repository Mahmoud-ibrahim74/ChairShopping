using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChairShopping.Migrations
{
    /// <inheritdoc />
    public partial class MakeCouponIdInPlaceOrderNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_placeOrders_coupons_CouponId",
                table: "placeOrders");

            migrationBuilder.AlterColumn<int>(
                name: "CouponId",
                table: "placeOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_placeOrders_coupons_CouponId",
                table: "placeOrders",
                column: "CouponId",
                principalTable: "coupons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_placeOrders_coupons_CouponId",
                table: "placeOrders");

            migrationBuilder.AlterColumn<int>(
                name: "CouponId",
                table: "placeOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_placeOrders_coupons_CouponId",
                table: "placeOrders",
                column: "CouponId",
                principalTable: "coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
