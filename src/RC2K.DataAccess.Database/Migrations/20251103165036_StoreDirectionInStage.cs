using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC2K.DataAccess.Database.Migrations
{
    /// <inheritdoc />
    public partial class StoreDirectionInStage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Users_UserId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Cars_CarId",
                table: "TimeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Drivers_DriverId",
                table: "TimeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Stages_StageId",
                table: "TimeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_VerifyInfos_VerifyInfoId",
                table: "TimeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_VerifyInfos_Users_VerifierId",
                table: "VerifyInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerifyInfos",
                table: "VerifyInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeEntries",
                table: "TimeEntries");

            migrationBuilder.RenameTable(
                name: "VerifyInfos",
                newName: "VerifyInfo");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "TimeEntries",
                newName: "TimeEntry");

            migrationBuilder.RenameColumn(
                name: "IsArcade",
                table: "Stages",
                newName: "Direction");

            migrationBuilder.RenameIndex(
                name: "IX_VerifyInfos_VerifierId",
                table: "VerifyInfo",
                newName: "IX_VerifyInfo_VerifierId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeEntries_VerifyInfoId",
                table: "TimeEntry",
                newName: "IX_TimeEntry_VerifyInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeEntries_StageId",
                table: "TimeEntry",
                newName: "IX_TimeEntry_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeEntries_DriverId",
                table: "TimeEntry",
                newName: "IX_TimeEntry_DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeEntries_CarId",
                table: "TimeEntry",
                newName: "IX_TimeEntry_CarId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Drivers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Drivers",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Drivers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "VerifierId",
                table: "VerifyInfo",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "VerifyInfo",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "User",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "User",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<Guid>(
                name: "VerifyInfoId",
                table: "TimeEntry",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "TimeEntry",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TimeEntry",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerifyInfo",
                table: "VerifyInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeEntry",
                table: "TimeEntry",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 1,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 2,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 3,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 4,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 5,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 6,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 7,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 8,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 9,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 10,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 11,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 12,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 13,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 14,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 15,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 16,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 17,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 18,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 19,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 20,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 21,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 22,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 23,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 24,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 25,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 26,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 27,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 28,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 29,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 30,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 31,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 32,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 33,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 34,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 35,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 36,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 37,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 38,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 39,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 40,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 41,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 42,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 43,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 44,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 45,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 46,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 47,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 48,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 49,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 50,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 51,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 52,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 53,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 54,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 55,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 56,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 57,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 58,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 59,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 60,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 61,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 62,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 63,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 64,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 65,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 66,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 67,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 68,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 69,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 70,
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 71,
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 72,
                column: "Direction",
                value: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_User_UserId",
                table: "Drivers",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntry_Cars_CarId",
                table: "TimeEntry",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntry_Drivers_DriverId",
                table: "TimeEntry",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntry_Stages_StageId",
                table: "TimeEntry",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntry_VerifyInfo_VerifyInfoId",
                table: "TimeEntry",
                column: "VerifyInfoId",
                principalTable: "VerifyInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VerifyInfo_User_VerifierId",
                table: "VerifyInfo",
                column: "VerifierId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_User_UserId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntry_Cars_CarId",
                table: "TimeEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntry_Drivers_DriverId",
                table: "TimeEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntry_Stages_StageId",
                table: "TimeEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntry_VerifyInfo_VerifyInfoId",
                table: "TimeEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_VerifyInfo_User_VerifierId",
                table: "VerifyInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerifyInfo",
                table: "VerifyInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeEntry",
                table: "TimeEntry");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Drivers");

            migrationBuilder.RenameTable(
                name: "VerifyInfo",
                newName: "VerifyInfos");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "TimeEntry",
                newName: "TimeEntries");

            migrationBuilder.RenameColumn(
                name: "Direction",
                table: "Stages",
                newName: "IsArcade");

            migrationBuilder.RenameIndex(
                name: "IX_VerifyInfo_VerifierId",
                table: "VerifyInfos",
                newName: "IX_VerifyInfos_VerifierId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeEntry_VerifyInfoId",
                table: "TimeEntries",
                newName: "IX_TimeEntries_VerifyInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeEntry_StageId",
                table: "TimeEntries",
                newName: "IX_TimeEntries_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeEntry_DriverId",
                table: "TimeEntries",
                newName: "IX_TimeEntries_DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeEntry_CarId",
                table: "TimeEntries",
                newName: "IX_TimeEntries_CarId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Drivers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "VerifierId",
                table: "VerifyInfos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "VerifyInfos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "VerifyInfoId",
                table: "TimeEntries",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                table: "TimeEntries",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TimeEntries",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerifyInfos",
                table: "VerifyInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeEntries",
                table: "TimeEntries",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 11,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 13,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 14,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 15,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 16,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 17,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 18,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 19,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 20,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 21,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 22,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 23,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 24,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 25,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 26,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 27,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 28,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 29,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 30,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 31,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 32,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 33,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 34,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 35,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 36,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 37,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 38,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 39,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 40,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 41,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 42,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 43,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 44,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 45,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 46,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 47,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 48,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 49,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 50,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 51,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 52,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 53,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 54,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 55,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 56,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 57,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 58,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 59,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 60,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 61,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 62,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 63,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 64,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 65,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 66,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 67,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 68,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 69,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 70,
                column: "IsArcade",
                value: false);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 71,
                column: "IsArcade",
                value: true);

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 72,
                column: "IsArcade",
                value: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Users_UserId",
                table: "Drivers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Cars_CarId",
                table: "TimeEntries",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Drivers_DriverId",
                table: "TimeEntries",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Stages_StageId",
                table: "TimeEntries",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_VerifyInfos_VerifyInfoId",
                table: "TimeEntries",
                column: "VerifyInfoId",
                principalTable: "VerifyInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VerifyInfos_Users_VerifierId",
                table: "VerifyInfos",
                column: "VerifierId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
