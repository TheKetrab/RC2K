using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RC2K.DataAccess.Database.Migrations
{
    /// <inheritdoc />
    public partial class bonuscars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Class", "Name" },
                values: new object[] { 18, "Max Power Car" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Class", "Name" },
                values: new object[,]
                {
                    { 24, 18, "MF Turbo" },
                    { 25, 18, "MF Hothatch" },
                    { 26, 18, "Bernie's Dicemobile" },
                    { 27, 18, "Skip" },
                    { 28, 18, "Turbotater" },
                    { 29, 18, "Moo 1.8BSE Turbo" },
                    { 30, 18, "Lambaaghini" },
                    { 31, 18, "Radio Car" },
                    { 32, 18, "Welsh Sheep Wagon" },
                    { 33, 18, "Local Shop" },
                    { 34, 18, "Clio" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Class", "Name" },
                values: new object[] { 8, "MF Turbo" });
        }
    }
}
