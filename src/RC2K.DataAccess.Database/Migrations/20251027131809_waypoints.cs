using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC2K.DataAccess.Database.Migrations
{
    /// <inheritdoc />
    public partial class waypoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 31,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "55.2491075,-3.2533364;55.2623553,-3.2553117;55.2742391,-3.261627;55.2706414,-3.2592452;55.2656658,-3.2535073;55.2665569,-3.2493774;55.2547615,-3.2474987;55.2457889,-3.2454704;55.2333417,-3.2460206;55.2404609,-3.2386813;55.2555436,-3.2362395;55.264432,-3.2413363;55.2691266,-3.2324528;55.2653856,-3.2295561" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 32,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "55.5799576,-2.8470598;55.5815481,-2.868296;55.5744477,-2.8748228;55.573461,-2.8886166;55.5774031,-2.8951713;55.5824665,-2.8943305;55.5893707,-2.9080469;55.5950664,-2.9080214;55.6025383,-2.8901209" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 33,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "55.6215321,-3.0942564;55.6133437,-3.0946008;55.6203456,-3.0974053;55.6227274,-3.1062642;55.6345814,-3.1147908;55.6339778,-3.1219431;55.6227377,-3.1145974;55.6156676,-3.117815;55.6109905,-3.1191319;55.6185788,-3.1215048;55.6277337,-3.1219804;55.6340654,-3.1254894" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 34,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "54.9915741,-4.4096625;54.9940791,-4.4085619;55.0039196,-4.396167;55.018781,-4.3734123;55.0236645,-4.3636705;55.0293507,-4.3527738;55.0311463,-4.3657343;55.0360652,-4.3698971;55.0406515,-4.3618206;55.0538223,-4.3668409;55.0658635,-4.3472072" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 35,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "55.1114868,-4.6426859;55.1304444,-4.6233196;55.1411236,-4.6149022;55.1507622,-4.6161897;55.1588269,-4.6301213;55.1726035,-4.6275893;55.1720857,-4.6019906;55.160703,-4.5960898;55.1503151,-4.5955149;55.1481937,-4.5926993;55.1484859,-4.5833043;55.1444888,-4.5823218;55.1320066,-4.5708257;55.1288823,-4.5784403;55.1252913,-4.5971009;55.1224381,-4.6028961;55.1061865,-4.6194355;55.0943557,-4.6173425;55.1047824,-4.5859976;55.1037088,-4.574991;55.0901618,-4.5724581;55.0792513,-4.5743539" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 36,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "bike", "55.2268885,-3.6352024;55.2217208,-3.6315674;55.2185621,-3.6221003;55.2179508,-3.6177368;55.2211677,-3.617941;55.2250101,-3.6192424;55.226058,-3.6189106;55.2329704,-3.6206734;55.2330286,-3.6178571;55.228794,-3.608627;55.2267129,-3.6031663;55.2224776,-3.6076829;55.2193165,-3.6105666;55.2179336,-3.6133225;55.2150076,-3.6138584;55.2071168,-3.6096184;55.2039945,-3.6096827;55.2021332,-3.607537;55.2055128,-3.6044256;55.2089656,-3.6005203;55.2142789,-3.6008422;55.2187714,-3.6091892;55.218906,-3.6057774;55.2192879,-3.6007538;55.2229254,-3.6006424;55.2250257,-3.5970871;55.2249401,-3.5957138;55.2306795,-3.5860578;55.234693,-3.5806076;55.2442111,-3.5709302;55.2465474,-3.5719172;55.2435872,-3.579642;55.2467187,-3.582174;55.2544729,-3.5782902;55.2591198,-3.5693852;55.2572855,-3.5655014;55.2546441,-3.5630552;55.251028,-3.5589941;55.2491081,-3.5530995;55.2481044,-3.5469243;55.2436823,-3.5418463;55.2407873,-3.542918;55.2342109,-3.5405193;55.2317081,-3.5411573;55.2297872,-3.5430711;55.2271652,-3.5453581;55.2280264,-3.5503181;55.2284517,-3.5527062;55.2302307,-3.5524663;55.2325444,-3.5568553;55.2309437,-3.5586415;55.2276403,-3.5611423;55.2216148,-3.5699458;55.2207706,-3.5765039;55.2159472,-3.5755178;55.2109085,-3.5727906;55.2082894,-3.5822397;55.2056248,-3.5833369;55.2030474,-3.5949474;55.1993282,-3.5957611;55.1957118,-3.5934518;55.1899437,-3.5886035;55.188385,-3.5835765" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 51,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "54.2695441,-5.9436646;54.2744611,-5.9516598;54.277376,-5.9690744;54.2800867,-5.9654936;54.2832844,-5.9588631;54.2871588,-5.9624096;54.2872168,-5.9476434;54.2976729,-5.9428498;54.3132324,-5.9487013;54.3241757,-5.9448057;54.3345806,-5.9465961;54.3407904,-5.942469;54.3451774,-5.935004;54.3558599,-5.9179497;54.3598739,-5.9075108" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 52,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "54.2102263,-6.3606529;54.2190401,-6.3633142;54.2264219,-6.3702173;54.2309064,-6.3685287;54.2361336,-6.3708844;54.2383631,-6.3812244;54.2383251,-6.3879784;54.2344646,-6.3901814;54.2319624,-6.3995735;54.2363806,-6.4000477;54.2394021,-6.3933767;54.2423015,-6.3800764;54.2489889,-6.383954;54.2563833,-6.391347;54.2566999,-6.3966213;54.2502027,-6.4059393;54.2521635,-6.4138816;54.2511755,-6.428105;54.255166,-6.4236172;54.2579435,-6.4161632;54.2600201,-6.4132316;54.264264,-6.4139728;54.2690647,-6.412953;54.2755007,-6.4089856;54.2828846,-6.4018213;54.289814,-6.3919248" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 53,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "54.4257316,-6.1650349;54.4271903,-6.1568478;54.428679,-6.1523937;54.4329029,-6.1545985;54.4352928,-6.157466;54.4415002,-6.1417045;54.445447,-6.1474803;54.4470748,-6.155164;54.4402303,-6.1616851;54.4344398,-6.1718923;54.4387374,-6.1816485;54.4343812,-6.189752;54.4331457,-6.2018448;54.4364948,-6.2079331;54.4438446,-6.2100906;54.445235,-6.2193526;54.4509796,-6.2238791;54.4568921,-6.2225654;54.4610105,-6.2406527;54.4665823,-6.2367736;54.4695446,-6.2123403;54.4703659,-6.2058873" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 54,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "54.489931,-6.9113891;54.4909936,-6.9116337;54.4966288,-6.909339;54.5036831,-6.8999815;54.5067101,-6.9087565;54.5031285,-6.9123935;54.4957244,-6.9311578;54.4957487,-6.937411;54.5023434,-6.9350269;54.5093916,-6.9236487;54.5106318,-6.9232709;54.5100352,-6.9163916;54.5072182,-6.9100547;54.5202185,-6.8944553;54.5197694,-6.8876165;54.5127161,-6.8839147;54.5099965,-6.890879;54.5038619,-6.8974968;54.5000972,-6.8931676;54.5039104,-6.8803474;54.4975953,-6.872693;54.503813,-6.8600696;54.4992589,-6.8520387" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 55,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "54.8128416,-6.7295615;54.81754,-6.7337886;54.8172436,-6.7487521;54.8153065,-6.7639324;54.8149188,-6.7795234;54.8202896,-6.7845561;54.8246029,-6.7718382;54.8282303,-6.769607;54.8414981,-6.7790779;54.8427258,-6.758997;54.8318105,-6.7513209;54.8236991,-6.7543702;54.8284121,-6.7422515" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 56,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "54.7237015,-6.1354;54.7321632,-6.147047;54.7418747,-6.1520021;54.7493214,-6.1476175;54.7516124,-6.1549793;54.7571991,-6.1698003;54.7647991,-6.1714603;54.7622382,-6.1832609;54.7536172,-6.1860396;54.7434054,-6.1775533;54.7489253,-6.1575969;54.7477172,-6.1553956;54.7403015,-6.1553956;54.7364681,-6.16135;54.7260286,-6.1578856" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 71,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "55.86020390016135,-2.404705916624326;55.86052336001392,-2.41880253919625;55.86602723773092,-2.426904690554126;55.87192908057129,-2.4371389870061795;55.878946320901825,-2.4348646989057228;55.885324529839565,-2.4232089723908845;55.89106402187944,-2.4168125371083504;55.8964838738952,-2.4023139504679416;55.903576359936224,-2.392221797022166" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 72,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "55.8613738,-2.5075883;55.8694904,-2.5104257;55.8797404,-2.5072733;55.8865856,-2.5069823;55.8927304,-2.5062688;55.8919817,-2.4966696;55.8761643,-2.4868177;55.8681212,-2.498876;55.8611062,-2.4927751;55.8579104,-2.4942655" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 73,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "55.8076268,-2.4766033;55.8201039,-2.4563524;55.8311692,-2.4333892;55.8174765,-2.3960529;55.8368886,-2.3934378;55.8449128,-2.3752417;55.8480299,-2.3807008" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 74,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "bike", "55.6978028,-2.4279197;55.6866311,-2.4188734;55.6801149,-2.4378057;55.6715428,-2.4267619;55.6707486,-2.4111336;55.675335,-2.4105145;55.6779441,-2.4115849;55.6794172,-2.4000355;55.668714,-2.3777032" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 75,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "55.7586322,-2.3469772;55.7629178,-2.3557926;55.7685173,-2.3544878;55.7652199,-2.3467925;55.7600104,-2.3427473" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 76,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "car", "55.7376067,-2.3320954;55.7369066,-2.3462381;55.7348744,-2.3636395;55.7350628,-2.3698927;55.737194,-2.3674458;55.7286223,-2.3873504;55.7303888,-2.3923697;55.7329559,-2.3921815;55.7339215,-2.3938964;55.7394555,-2.3898601;55.7455184,-2.371184;55.745436,-2.3652236;55.7397616,-2.3643452;55.7442588,-2.3452927;55.7451523,-2.3362839" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 31,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 32,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 33,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 34,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 35,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 36,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 51,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 52,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 53,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 54,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 55,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 56,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

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
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 73,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 74,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 75,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "StageWaypoints",
                keyColumn: "StageCode",
                keyValue: 76,
                columns: new[] { "ApiHint", "Waypoints" },
                values: new object[] { "", "" });
        }
    }
}
