using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicECommerce_API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VariantName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VariantValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProductImages_ProductId",
            //    table: "ProductImages",
            //    column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_Unique",
                table: "ProductVariants",
                columns: new[] { "ProductId", "VariantName", "VariantValue" },
                unique: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductImages_Products_ProductId",
            //    table: "ProductImages",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductImages_Products_ProductId",
            //    table: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProductImages_ProductId",
            //    table: "ProductImages");
        }
    }
}
