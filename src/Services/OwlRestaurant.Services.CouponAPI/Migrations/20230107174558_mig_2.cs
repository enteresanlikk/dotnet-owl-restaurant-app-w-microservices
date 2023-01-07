using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OwlRestaurant.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "DiscountAmount" },
                values: new object[,]
                {
                    { new Guid("341918dd-12ef-49f6-8eaf-68ceffaef70f"), "20FF", 20.0 },
                    { new Guid("86b0d6d7-912a-4da0-8591-221274e870ce"), "10FF", 10.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("341918dd-12ef-49f6-8eaf-68ceffaef70f"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("86b0d6d7-912a-4da0-8591-221274e870ce"));
        }
    }
}
