using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicECommerce_API.Migrations
{
    /// <inheritdoc />
    public partial class FixCouponIsActiveColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Coupons",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Coupons",
                newName: "isActive");
        }
    }
}
