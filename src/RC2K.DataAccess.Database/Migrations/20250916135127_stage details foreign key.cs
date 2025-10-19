using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC2K.DataAccess.Database.Migrations
{
    /// <inheritdoc />
    public partial class stagedetailsforeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StagesData_StageDetails_StageDetailsStageCode",
                table: "StagesData");

            migrationBuilder.DropIndex(
                name: "IX_StagesData_StageDetailsStageCode",
                table: "StagesData");

            migrationBuilder.DropColumn(
                name: "StageDetailsStageCode",
                table: "StagesData");

            migrationBuilder.AlterColumn<int>(
                name: "StageCode",
                table: "StageDetails",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_StageDetails_StagesData_StageCode",
                table: "StageDetails",
                column: "StageCode",
                principalTable: "StagesData",
                principalColumn: "StageCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StageDetails_StagesData_StageCode",
                table: "StageDetails");

            migrationBuilder.AddColumn<int>(
                name: "StageDetailsStageCode",
                table: "StagesData",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StageCode",
                table: "StageDetails",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

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
    }
}
