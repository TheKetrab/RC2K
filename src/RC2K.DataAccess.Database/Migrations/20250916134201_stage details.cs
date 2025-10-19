using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RC2K.DataAccess.Database.Migrations
{
    /// <inheritdoc />
    public partial class stagedetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StageDetailsStageCode",
                table: "StagesData",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StageDetails",
                columns: table => new
                {
                    StageCode = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Length = table.Column<float>(type: "REAL", nullable: false),
                    Asphalt = table.Column<int>(type: "INTEGER", nullable: false),
                    Dirt = table.Column<int>(type: "INTEGER", nullable: false),
                    Gravel = table.Column<int>(type: "INTEGER", nullable: false),
                    Snow = table.Column<int>(type: "INTEGER", nullable: false),
                    Temp = table.Column<int>(type: "INTEGER", nullable: false),
                    Wind = table.Column<float>(type: "REAL", nullable: false),
                    Mood = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageDetails", x => x.StageCode);
                });

            migrationBuilder.InsertData(
                table: "StageDetails",
                columns: new[] { "StageCode", "Asphalt", "Dirt", "Gravel", "Length", "Mood", "Snow", "Temp", "Wind" },
                values: new object[,]
                {
                    { 21, 100, 0, 0, 10.17f, 2, 0, 31, 2.4f },
                    { 22, 99, 1, 0, 14.02f, 2, 0, 29, 6.4f },
                    { 23, 77, 0, 23, 17.85f, 4, 0, 26, 0.3f },
                    { 24, 98, 0, 2, 6.6f, 4, 0, 24, 0f },
                    { 25, 99, 0, 1, 18.25f, 8, 0, 22, 0f },
                    { 26, 98, 1, 1, 22.3f, 8, 0, 21, 0.2f },
                    { 31, 7, 0, 93, 9.08f, 8, 0, 7, 0.3f },
                    { 32, 0, 0, 100, 8.87f, 1, 0, 12, 0f },
                    { 33, 0, 0, 100, 10.27f, 2, 0, 16, 0.2f },
                    { 34, 0, 0, 100, 9.82f, 258, 0, 12, 0.8f },
                    { 35, 0, 0, 100, 29.71f, 258, 0, 6, 6.1f },
                    { 36, 0, 0, 100, 30.42f, 514, 0, 4, 18.8f },
                    { 41, 0, 5, 95, 19.31f, 2, 0, 13, 1.9f },
                    { 42, 0, 0, 100, 15.3f, 2, 0, 10, 6f },
                    { 43, 0, 0, 100, 24.36f, 258, 0, 7, 6.1f },
                    { 44, 0, 0, 0, 24.33f, 1026, 100, 2, 6f },
                    { 45, 0, 0, 0, 26.34f, 1028, 100, 0, 12.9f },
                    { 46, 0, 0, 0, 20.39f, 1032, 100, -2, 21.4f },
                    { 51, 97, 3, 0, 15.69f, 513, 0, 14, 46.2f },
                    { 52, 97, 3, 0, 18.76f, 1, 0, 16, 25.6f },
                    { 53, 96, 1, 3, 17.86f, 2, 0, 19, 17.7f },
                    { 54, 74, 1, 25, 18.09f, 2, 0, 24, 16.7f },
                    { 55, 74, 1, 25, 14.66f, 2, 0, 26, 6.6f },
                    { 56, 88, 5, 7, 15f, 2, 0, 23, 3.7f },
                    { 61, 0, 0, 100, 38.78f, 8, 0, 3, 6.4f },
                    { 62, 0, 0, 100, 29.43f, 2, 0, 12, 4.7f },
                    { 63, 0, 0, 100, 29.76f, 2, 0, 10, 5.1f },
                    { 64, 0, 0, 100, 42.82f, 2050, 0, 5, 19.5f },
                    { 65, 0, 0, 100, 14.16f, 2050, 0, 4, 20f },
                    { 66, 0, 0, 100, 29.32f, 514, 0, 3, 16.1f },
                    { 71, 95, 1, 4, 7.26f, 4, 0, 22, 1.3f },
                    { 72, 90, 1, 9, 10.51f, 4, 0, 21, 1.1f },
                    { 73, 91, 0, 9, 11.72f, 4, 0, 19, 1.1f },
                    { 74, 85, 2, 13, 9.99f, 8, 0, 14, 1.6f },
                    { 75, 80, 6, 14, 3.25f, 8, 0, 12, 1.9f },
                    { 76, 83, 0, 17, 11.65f, 8, 0, 10, 1.8f }
                });

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 21,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 22,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 23,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 24,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 25,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 26,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 31,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 32,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 33,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 34,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 35,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 36,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 41,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 42,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 43,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 44,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 45,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 46,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 51,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 52,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 53,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 54,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 55,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 56,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 61,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 62,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 63,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 64,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 65,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 66,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 71,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 72,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 73,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 74,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 75,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "StagesData",
                keyColumn: "StageCode",
                keyValue: 76,
                column: "StageDetailsStageCode",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_StagesData_StageDetailsStageCode",
                table: "StagesData",
                column: "StageDetailsStageCode");

            migrationBuilder.AddForeignKey(
                name: "FK_StagesData_StageDetails_StageDetailsStageCode",
                table: "StagesData",
                column: "StageDetailsStageCode",
                principalTable: "StageDetails",
                principalColumn: "StageCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StagesData_StageDetails_StageDetailsStageCode",
                table: "StagesData");

            migrationBuilder.DropTable(
                name: "StageDetails");

            migrationBuilder.DropIndex(
                name: "IX_StagesData_StageDetailsStageCode",
                table: "StagesData");

            migrationBuilder.DropColumn(
                name: "StageDetailsStageCode",
                table: "StagesData");
        }
    }
}
