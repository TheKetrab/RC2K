using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC2K.DataAccess.Database.Migrations
{
    /// <inheritdoc />
    public partial class ApiHints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPathValid",
                table: "StageWaypoints");

            migrationBuilder.AddColumn<string>(
                name: "ApiHint",
                table: "StageWaypoints",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 21,
                column: "ApiHint",
                value: "bike");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 22,
                column: "ApiHint",
                value: "car");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 23,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "bike", "54.2925137,-4.5794562;54.3049822,-4.575546;54.3030134,-4.5671377;54.3301914,-4.5553174;54.3299322,-4.5537086;54.3207096,-4.5492764;54.3219861,-4.5427103;54.3196834,-4.5364447;54.3234629,-4.5338698;54.327267,-4.5310803;54.3269167,-4.5462294;54.3299948,-4.5507784;54.3388275,-4.55198;54.3379268,-4.53932;54.3355769,-4.5327248;54.3312438,-4.5021903;54.3324972,-4.48343;54.323525,-4.478758" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 24,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "54.3156717,-4.4849128;54.3151715,-4.4887726;54.3073269,-4.4898721;54.2995679,-4.4853024;54.2764154,-4.4935351;54.2708857,-4.4947851;54.2664541,-4.483406" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 25,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "54.2392179,-4.6001656;54.2438606,-4.577607;54.2523886,-4.5775069;54.2471995,-4.5652364;54.2474655,-4.5387513;54.2314794,-4.5224346;54.2007131,-4.5256629;54.2028513,-4.5226207;54.2053066,-4.5174728;54.2051566,-4.5116617;54.1944018,-4.5136428;54.1828935,-4.5065698;54.1872281,-4.4941888;54.1839937,-4.4765511" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 26,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "54.1015818,-4.6833901;54.1102406,-4.6949709;54.1166429,-4.6885336;54.1135748,-4.682729;54.1019256,-4.6802267;54.1092891,-4.6676369;54.1173622,-4.6557725;54.127605,-4.661999;54.1351724,-4.6677933;54.1312624,-4.6824002;54.119058,-4.6872147;54.1216356,-4.6869764;54.1276671,-4.689926;54.136638,-4.687929;54.1481165,-4.6859442;54.1637829,-4.6670789;54.1698884,-4.6659241;54.178584,-4.6544162;54.1996249,-4.6429132" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 31,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 32,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 33,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 34,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 35,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 36,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 41,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 42,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 43,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 44,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 45,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 46,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 51,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 52,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 53,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 54,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 55,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 56,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 61,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 62,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 63,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 64,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 65,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 66,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 71,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "\"55.86020390016135", "2.404705916624326;55.86052336001392,-2.41880253919625;55.86602723773092,-2.426904690554126;55.87192908057129,-2.4371389870061795;55.878946320901825,-2.4348646989057228;55.885324529839565,-2.4232089723908845;55.89106402187944,-2.4168125371083504;55.8964838738952,-2.4023139504679416;55.903576359936224,-2.392221797022166" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 72,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 73,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 74,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 75,
                column: "ApiHint",
                value: "");

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 76,
                column: "ApiHint",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiHint",
                table: "StageWaypoints");

            migrationBuilder.AddColumn<bool>(
                name: "IsPathValid",
                table: "StageWaypoints",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 21,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 22,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 23,
                columns: new[] { "IsPathValid", "Waypoints" },
                values: new object[] { false, "4.29255562,-4.5794437;54.3026520,-4.579188578701743;54.3082203634176, -4.564388361708989;54.32658008948149, -4.551420821392586;54.321536770938, -4.543763444294593;54.31965949537865, -4.535309122112026;54.32789384120656, -4.53076009596303;54.327568503197114, -4.544235513045905;54.33907889824062, -4.549342439016684;54.3307263753353, -4.520152862249664;54.33175265892771, -4.493145162323119;54.32428497604704, -4.478518521109954" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 24,
                columns: new[] { "IsPathValid", "Waypoints" },
                values: new object[] { false, "4.31564618,-4.4848432;54.2664736,-4.483444" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 25,
                columns: new[] { "IsPathValid", "Waypoints" },
                values: new object[] { false, "4.23917280,-4.6002638;54.2006546,-4.5256867;54.1839275,-4.4764227" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 26,
                columns: new[] { "IsPathValid", "Waypoints" },
                values: new object[] { false, "4.10160104428731, -4.683293842124824;54.113896384972584, -4.691513482013081;54.10584830157522, -4.673447010173375;54.126688718956444, -4.661590025329006;54.19422602624327, -4.642505230241718;54.1996597561764, -4.64286247684897" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 31,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 32,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 33,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 34,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 35,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 36,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 41,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 42,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 43,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 44,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 45,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 46,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 51,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 52,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 53,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 54,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 55,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 56,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 61,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 62,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 63,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 64,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 65,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 66,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 71,
                columns: new[] { "IsPathValid", "Waypoints" },
                values: new object[] { false, "55.86020390016135,-2.404705916624326;55.86052336001392,-2.41880253919625;55.86602723773092,-2.426904690554126;55.87192908057129,-2.4371389870061795;55.878946320901825,-2.4348646989057228;55.885324529839565,-2.4232089723908845;55.89106402187944,-2.4168125371083504;55.8964838738952,-2.4023139504679416;55.903576359936224,-2.392221797022166" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 72,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 73,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 74,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 75,
                column: "IsPathValid",
                value: false);

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 76,
                column: "IsPathValid",
                value: false);
        }
    }
}
