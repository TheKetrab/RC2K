using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC2K.DataAccess.Database.Migrations
{
    /// <inheritdoc />
    public partial class fix_71_waypoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 71,
                column: "Waypoints",
                value: "55.86020390016135,-2.404705916624326;55.86052336001392,-2.41880253919625;55.86602723773092,-2.426904690554126;55.87192908057129,-2.4371389870061795;55.878946320901825,-2.4348646989057228;55.885324529839565,-2.4232089723908845;55.89132824170699,-2.4148295104601187;55.8964838738952,-2.4023139504679416;55.903576359936224,-2.392221797022166");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 71,
                column: "Waypoints",
                value: "55.86020390016135,-2.404705916624326;55.86052336001392,-2.41880253919625;55.86602723773092,-2.426904690554126;55.87192908057129,-2.4371389870061795;55.878946320901825,-2.4348646989057228;55.885324529839565,-2.4232089723908845;55.89106402187944,-2.4168125371083504;55.8964838738952,-2.4023139504679416;55.903576359936224,-2.392221797022166");
        }
    }
}
