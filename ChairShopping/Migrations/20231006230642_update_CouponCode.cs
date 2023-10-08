using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChairShopping.Migrations
{
    /// <inheritdoc />
    public partial class update_CouponCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CouponCode",
                table: "coupons",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CouponCode",
                table: "coupons",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "int");
        }
    }
}
