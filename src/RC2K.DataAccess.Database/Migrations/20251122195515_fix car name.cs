using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC2K.DataAccess.Database.Migrations
{
    /// <inheritdoc />
    public partial class fixcarname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeEntry");

            migrationBuilder.DropTable(
                name: "VerifyInfo");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "User",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 21,
                column: "Name",
                value: "Ford Escort Maxi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "VerifyInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    VerifierId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    VerifyDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifyInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerifyInfo_User_VerifierId",
                        column: x => x.VerifierId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CarId = table.Column<int>(type: "INTEGER", nullable: false),
                    DriverId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StageId = table.Column<int>(type: "INTEGER", nullable: false),
                    VerifyInfoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    UploadTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeEntry_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeEntry_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeEntry_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeEntry_VerifyInfo_VerifyInfoId",
                        column: x => x.VerifyInfoId,
                        principalTable: "VerifyInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 21,
                column: "Name",
                value: "Ford Escort RS2000");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntry_CarId",
                table: "TimeEntry",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntry_DriverId",
                table: "TimeEntry",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntry_StageId",
                table: "TimeEntry",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntry_VerifyInfoId",
                table: "TimeEntry",
                column: "VerifyInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_VerifyInfo_VerifierId",
                table: "VerifyInfo",
                column: "VerifierId");
        }
    }
}
