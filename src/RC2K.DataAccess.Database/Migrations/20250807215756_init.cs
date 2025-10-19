using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RC2K.DataAccess.Database.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Class = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StagesData",
                columns: table => new
                {
                    StageCode = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImgName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagesData", x => x.StageCode);
                });

            migrationBuilder.CreateTable(
                name: "StageWaypoints",
                columns: table => new
                {
                    StageCode = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Waypoints = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    IsPathValid = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageWaypoints", x => x.StageCode);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false),
                    Roles = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<int>(type: "INTEGER", nullable: false),
                    IsArcade = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages_StageWaypoints_Code",
                        column: x => x.Code,
                        principalTable: "StageWaypoints",
                        principalColumn: "StageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stages_StagesData_Code",
                        column: x => x.Code,
                        principalTable: "StagesData",
                        principalColumn: "StageCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Known = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Key = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VerifyInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VerifierId = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    VerifyDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifyInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerifyInfos_Users_VerifierId",
                        column: x => x.VerifierId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StageId = table.Column<int>(type: "INTEGER", nullable: false),
                    CarId = table.Column<int>(type: "INTEGER", nullable: false),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false),
                    Time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    UploadTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VerifyInfoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeEntries_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeEntries_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeEntries_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeEntries_VerifyInfos_VerifyInfoId",
                        column: x => x.VerifyInfoId,
                        principalTable: "VerifyInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Class", "Name" },
                values: new object[,]
                {
                    { 1, 7, "Renault Mégane" },
                    { 2, 7, "Vauxhall Astra" },
                    { 3, 8, "Seat Cordoba WRC" },
                    { 4, 7, "Hyundai Coupé Evo II" },
                    { 5, 8, "Subaru Impreza WRC" },
                    { 6, 6, "Peugeot 106 Maxi" },
                    { 7, 8, "MF Turbo" },
                    { 8, 8, "Mitsubishi Lancer Evo V" },
                    { 9, 7, "Volkswagen Golf GTI MKIV" },
                    { 10, 7, "Nissan Almera" },
                    { 11, 8, "Proton Wira/Persona" },
                    { 12, 6, "Citroën Saxo" },
                    { 13, 6, "Honda Civic" },
                    { 14, 5, "Nissan Micra Maxi" },
                    { 15, 8, "Peugeot 206 WRC" },
                    { 16, 7, "Ford Escort RS2000" },
                    { 17, 7, "Škoda Octavia" },
                    { 18, 7, "Seat Ibiza Cupra Sport" },
                    { 19, 6, "Škoda Felicia" },
                    { 20, 6, "Proton Compact" },
                    { 21, 7, "Ford Escort RS2000" },
                    { 22, 5, "Ford Puma" },
                    { 23, 8, "Mitsubishi Lancer Evo IV" }
                });

            migrationBuilder.InsertData(
                table: "StageWaypoints",
                columns: new[] { "StageCode", "IsPathValid", "Path", "Waypoints" },
                values: new object[,]
                {
                    { 21, false, null, "54.1412097,-4.4701409;54.1355627,-4.4864447;54.1277141,-4.5221752;54.1266989,-4.5372641;54.114791,-4.569775;54.120478,-4.57988" },
                    { 22, false, null, "54.2262301,-4.6734796;54.2282370,-4.6504770;54.2179001,-4.6418939;54.2319497,-4.6293626;54.242985,-4.618033;54.248401,-4.6355424;54.2596334,-4.6223245;54.2722654,-4.6018968;54.2678098,-4.5894544;54.2610085,-4.5824171" },
                    { 23, false, null, "4.29255562,-4.5794437;54.3026520,-4.579188578701743;54.3082203634176, -4.564388361708989;54.32658008948149, -4.551420821392586;54.321536770938, -4.543763444294593;54.31965949537865, -4.535309122112026;54.32789384120656, -4.53076009596303;54.327568503197114, -4.544235513045905;54.33907889824062, -4.549342439016684;54.3307263753353, -4.520152862249664;54.33175265892771, -4.493145162323119;54.32428497604704, -4.478518521109954" },
                    { 24, false, null, "4.31564618,-4.4848432;54.2664736,-4.483444" },
                    { 25, false, null, "4.23917280,-4.6002638;54.2006546,-4.5256867;54.1839275,-4.4764227" },
                    { 26, false, null, "4.10160104428731, -4.683293842124824;54.113896384972584, -4.691513482013081;54.10584830157522, -4.673447010173375;54.126688718956444, -4.661590025329006;54.19422602624327, -4.642505230241718;54.1996597561764, -4.64286247684897" },
                    { 31, false, null, "" },
                    { 32, false, null, "" },
                    { 33, false, null, "" },
                    { 34, false, null, "" },
                    { 35, false, null, "" },
                    { 36, false, null, "" },
                    { 41, false, null, "" },
                    { 42, false, null, "" },
                    { 43, false, null, "" },
                    { 44, false, null, "" },
                    { 45, false, null, "" },
                    { 46, false, null, "" },
                    { 51, false, null, "" },
                    { 52, false, null, "" },
                    { 53, false, null, "" },
                    { 54, false, null, "" },
                    { 55, false, null, "" },
                    { 56, false, null, "" },
                    { 61, false, null, "" },
                    { 62, false, null, "" },
                    { 63, false, null, "" },
                    { 64, false, null, "" },
                    { 65, false, null, "" },
                    { 66, false, null, "" },
                    { 71, false, null, "55.86020390016135,-2.404705916624326;55.86052336001392,-2.41880253919625;55.86602723773092,-2.426904690554126;55.87192908057129,-2.4371389870061795;55.878946320901825,-2.4348646989057228;55.885324529839565,-2.4232089723908845;55.89106402187944,-2.4168125371083504;55.8964838738952,-2.4023139504679416;55.903576359936224,-2.392221797022166" },
                    { 72, false, null, "" },
                    { 73, false, null, "" },
                    { 74, false, null, "" },
                    { 75, false, null, "" },
                    { 76, false, null, "" }
                });

            migrationBuilder.InsertData(
                table: "StagesData",
                columns: new[] { "StageCode", "Description", "ImgName", "Name" },
                values: new object[,]
                {
                    { 21, "Finałowy etap Rajdu Walii jest stosunkowo kręty i pro wadzony w ciemności, ale doświadczeni kierowcy i tak mogą osiągać duże prędkości. Jest bardziej nierówny niż Dyfi, co zwiększa prawdopodobieństwo przebicia opony, ale szybsze odcinki są darem od Boga dla kierowców posiadających samochody o mniejszej mocy.", "MNX_001.PNG", "Port Soderick" },
                    { 22, "Prawdziwie międzynarodowy etap Newcastleton rozpo czyna się w Szkocji, następnie przecina rzekę i wchodzi do Anglii. Sekcja początkowa jest ciasna, po czym kierowcy wchodzą na szybszy fragment drogi, która jest używana również w etapie Kershope. Następnie etap przechodzi na niziny z szybszymi i równiejszymi drogami.", "MNX_002.PNG", "Ballagyr" },
                    { 23, "Najdłuższy etap w rajdzie. Ae był świadkiem kilku drama tycznych zakończeń i potrafi postawić na głowie tabelę wyników. Twarda nawierzchnia i otwarte zakręty spra wiają, że jest lubiany zarówno przez kierowców, jak i kibi ców, chociaż największe tłumy gromadzą się przy cias nych wirażach w lewo.", "MNX_003.PNG", "Curraghs" },
                    { 24, "Końcowy etap posiada wszystkiego po trochu, a w tym ekstremalnie szybkie sekcje na gładkich drogach oraz serie wiraży wymagających cierpliwości i koncentracji. Również tu droga może być pokryta pozostałościami po pracach polowych, a w ciemności nocy trudno jest ocenić poziom przyczepności samochodu.", "MNX_004.PNG", "Tholt-y-Will" },
                    { 25, "Większość etapów w rejonie płaskowyżu Antrim jest uznawana za zbyt szybkie dla współczesnych samocho dów rajdowych, ale Tardree jakoś udało się przeforsować. Jest to ciągle najszybszy sprawdzian rajdu. Super szybki finał jest klasycznym górskim etapem. Zawiera niewiele skrzyżowań i kilka bardzo szybkich prostych.", "MNX_005.PNG", "Injerbreck" },
                    { 26, "Mistrzostwa Mobil 1 kończą się jednym z najtrudniejszych etapów. Ponad 20 km łączących w sobie szybkie, otwarte drogi kategorii B, wiejskie drogi i skomplikowane rozjazdy, a to wszystko jeszcze utrudnione ciemnością i ciężkim deszczem. Nie można wyobrazić sobie lepszego testu, który zdecyduje o tym, kto zgarnie wszystko.", "MNX_006.PNG", "Cringle" },
                    { 31, "Będąc odzwierciedleniem klasycznego Rajdu Walii, Myth erin posiada kręty odcinek startowy wyprowadzający zawodników na sam grzbiet wzniesienia, aby po chwili rzucić ich na dno doliny. Środkowy odcinek rajdu jest szybki i pozwala na płynną jazdę, ale posiada mnóstwo wzniesień wymagających pełnego zaangażowania.", "SC_001.PNG", "Twiglees" },
                    { 32, "Kilkukilometrowy początek Kershope zawiera jeden naj bardziej wymagających, leśnych odcinków rajdowych w Wielkiej Brytanii. Droga wydaje się być znacznie bardziej kręta niż jest naprawdę i odważniejsi kierowcy mogą w rezultacie znacznie nadrobić czas. Mgła w wyższych partiach wymaga od kierowców wzmożonej koncentracji.", "SC_002.PNG", "Yair" },
                    { 33, "Niezwykle malowniczy etap Cardrona rozpoczyna się powolnym podjazdem pod wzniesienie, który kończy ostry zakręt w prawo. Środkowa część tego odcinka jest gęsto zalesiona, a droga jest szybsza i pozwala na bardziej płynną jazdę. Sekcja zjazdowa zwiększa prędkość i prowadzi zawodników w kierunku mety.", "SC_003.PNG", "Cardrona" },
                    { 34, "Rozpoczynając się w pobliżu wioski Longformacus na wąs kiej wiejskiej drodze, etap ten wykorzystuje szeroką i szybką drogę kategorii B i kończy się na Abbey Road. Jest to również ostatni etap rajdu rozgrywany w świe tle dziennym, tak więc niedoświadczeni nocni kierowcy powinni atakować, kiedy tylko mogą.", "SC_004.PNG", "Black Loch" },
                    { 35, "Trzeci sprawdzian jest jednym z najbardziej męczących dla kierowców, gdyż nie posiada zbyt wielu prostych dających im odpocząć po serii wiraży i zakrętów. Mimo, że oparty jest na stosunkowo gładkiej nawierzchni, etap ten zawiera wiele rozjazdów i zwodniczych łuków, które mogą zmylić nieuważnych. Kończy się na Feeney Road.", "SC_005.PNG", "Glentrool" },
                    { 36, "Jako jeden z klasycznych etapów na wyspie Man, Cur raghs jest niezwykle wyboisty, bardzo wąski i niesamo wicie szybki. Ma kręte odcinki, ale średnie prędkości wciąż są wysokie. Uważaj na jego najsławniejszą cechę: doły i skoki w pobliżu mety, gdzie sprawdzian przecina główną drogę Jurby.", "SC_006.PNG", "Ae" },
                    { 41, "Jest jednym z najtrudniejszych odcinków w lasach Walii. Obfituje we wzniesienia, a jego drogi są znacznie węższe od normalnych. Skrzyżowanie w Clocaenog potrafi być błotniste, a ostatnie zalecenia Komisji Leśnictwa znacznie utrudniły odczytywanie drogi.", "VRW_001.PNG", "Clocaenog Mid" },
                    { 42, "To bardzo szybkie wprowadzenie do leśnego kompleksu Kieldera posiada wszystkie cechy tego legendarnego i rzucającego wyzwanie miejsca. Jest tu wiele długich, szybkich prostych ze ślepymi wzniesieniami i wąski most sprzyjający najodważniejszym kierowcom. Głębokie rowy są zdolne pochłonąć wypadający z drogi samochód.", "VRW_002.PNG", "Penmachno South" },
                    { 43, "Rezerwat Black Esk jest miejscem otwierającym etap, który rozpoczyna się od jazdy wzdłuż rzeki Esk w sza rówce wczesnego poranka. Następnie występuje gęsto zadrzewiona sekcja posiadająca wiele szybkich prostych i przecinek leśnych, które testują nerwy kierowców. Jest to etap cieszący się dużą popularnością wśród kibiców.", "VRW_003.PNG", "Myherin" },
                    { 44, "Pierwszy asfaltowy etap rajdowych mistrzostw Wielkiej Brytanii jest jak dynamit. Krótki, ostry i wąski. Premio wane tutaj będzie panowanie nad samochodem i konfigu racja zawieszenia, gdyż na drodze często porozrzu cane są pozostałości prac rolniczych. Uważaj, aby nie uderzyć w dom znajdujący się w pobliżu mety.", "VRW_004.PNG", "Hafren" },
                    { 45, "Jeden z klasycznych etapów w Północnej Irlandii, Hamil ton's Folly to prawie 16 km wyboistej, wąskiej drogi w górach Mourne. Ze swoimi wielkimi wyskoczniami jest trudnym początkiem zarówno dla kierowcy, jak i pilota, zwłaszcza w mokrych warunkach. Wiele załóg wypadało już na pierwszej przeszkodzie.", "VRW_005.PNG", "Dyfi" },
                    { 46, "Pierwszy etap ostatniej rundy rajdu Mobil 1 jest dobrym wprowadzeniem do Manx i jednocześnie surową próbą. Rozpoczyna się na malowniczym moście i biegnie kilka kilometrów wzdłuż morza, po czym wcina się w głąb lądu do szybszych dróg w pobliżu mety.", "VRW_006.PNG", "Gartheiniog" },
                    { 51, "Odcinek ten wykorzystuje w paru miejscach bardzo bliskie rozjazdy, tak że kierowcy będą odwiedzać te same miej sca kilkukrotnie. Mimo to Dyfi jest bardzo lubiany. Zaśnieżone drogi są niezwykle śliskie i wymagają dosko nałego panowania nad pojazdem, zwłaszcza gdy samo chody wspinają się serpentynami na dwa wzniesienia.", "UL_001.PNG", "Hamilton's Folly" },
                    { 52, "Jeżeli tylko w Kieldar jest jakiś łatwy etap, to Riccarton zasługuje na to miano. Jest krótki, posiada kręty start i szybkie, szerokie zakończenie. Ze względu na znacznie szersze drogi i płytkie rowy zmniejsza się również praw dopodobieństwo nieszczęśliwego wypadku.", "UL_002.PNG", "Tyrones Ditches" },
                    { 53, "Po czterech krótkich sprawdzianach, Glentrool sprowa dza kierowców na ziemię swoimi 29 najtrudniejszymi, leśnymi kilometrami w Szkocji. Jego drogi są stosunkowo szybkie, ale można jeszcze przyspieszyć jazdę ścinając wybrane zakręty. Etap przyspiesza zbliżając się do koń ca lotnym finiszem premiującym umiejętności i odwagę.", "UL_003.PNG", "Feeney" },
                    { 54, "Mając niewiele ponad 3 km, Lengton jest najkrótszym etapem w serii Mobil 1, ale kierowcy i tak nie mogą na nim odpocząć. Na swoim krótkim dystansie ma odrobinę wszystkiego, w tym również serię wiraży i kilka sko czni. Odcinek jest lubiany przez kibiców, więc w ciem ności mogą przeszkadzać błyski fotograficznych fleszy.", "UL_004.PNG", "Parkanaur" },
                    { 55, "Etap ten daje kierowcy fałszywe poczucie bezpie czeństwa z kilkoma szybkimi prostymi na początkowych kilometrach, ale wkrótce staje się znacznie trudniejszy, gdy załogi trafiają na długie, kręte drogi wiodące w górę i w dół gór Coolnasillagh. Wracając na niziny, samochody trafiają na wąskie drogi poprzeplatane szybkimi prostymi.", "UL_005.PNG", "Lisnamuck" },
                    { 56, "W ciągu dnia Injebreck jest jednym z najbardziej wymaga jących etapów Manx. W ciemności potrafi załamać sezon. Posiada bardzo szybkie, górskie drogi poprzecinane os trymi serpentynami. Uważaj na ostry wiraż w prawo tuż po starcie, gdyż inaczej będziesz musiał złapać najbliższy prom do domu.", "UL_006.PNG", "Tardree" },
                    { 61, "Jest to górzysty odcinek, który dwukrotnie prowadzi  wokół zboczy góry na dwóch różnych wysokościach. Drogi są niezwykle szybkie, ale największe prędkości często ograniczane są drzewami rosnącymi blisko trasy. Nawierzchnia pokryta jest ostrymi krzemieniami, co sprawia, że walka często kończy się złapaniem gumy.", "PIR_001.PNG", "Chirdonhead" },
                    { 62, "Drugi etap pozwala znacznie rozpędzić się już od linii star towej, posiadając jeden z najszybszych odcinków dróg w Kielder. Falstone wspina się do najwyższego punktu w całym kompleksie i jest lubianym odcinkiem, ale jego nawierzchnia może być bardziej ostra niż zazwyczaj, powodując przebicia opon.", "PIR_002.PNG", "Falstone" },
                    { 63, "Etap ten wymaga od załóg jazdy wokół południowego i zachodniego stoku wzniesienia. Jest jednym z najszyb szych w rajdzie, mimo że średnie prędkości mogą być obniżane z powodu słabej porannej widoczności. Będziesz miał szczęście, jeżeli zobaczysz choćby jednego kibica, gdyż nie można się tu dostać z głównej drogi.", "PIR_003.PNG", "Kershope" },
                    { 64, "Bothwell jest kolejnym szkockim sprawdzianem wymaga jącym wysokiego poziomu koncentracji. Posiada bardziej kręte fragmenty i ciasne wiraże w pobliżu Crichness, ale kierowcy będą raczej pamiętać fenomenalne prędkości osiągane na najszybszych prostych w pobliżu mety.", "PIR_004.PNG", "Pundershaw" },
                    { 65, "Ten południowy, etap zlokalizowany w pobliżu granicznego miasta Newry, jest wielką próbą dla kierowców. Ma szyb ki start, ale wkrótce trasa zwęża się na krętych dro gach, które mogą być zdradliwe dzięki nieprzewidywalnej irlandzkiej pogodzie. Ostatnie kilometry są bardzo szyb kie i prowadzą po szerokim, gładkim asfalcie.", "PIR_005.PNG", "Riccarton" },
                    { 66, "Większość tego etapu rozpoczynającego się tuż za mias tem Peel, wiedzie przez szybkie, wąskie drogi, ale prawie 5 km sprawdzianu prowadzi przez szerszą, główną drogę. W Ostatnich latach na etapie Ballagyr wybili się jedni z największych mistrzów Mobil 1 BRC, wśród nich m.in. Gwyndaf Evans i Alister McRae w 1997 roku.", "PIR_006.PNG", "Newcastleton" },
                    { 71, "Kompleks Hafren daje odcinkowi wiele możliwości i jest jednym z najpopularniejszych miejsc w Walii nawet podczas opadów śniegu. Otwierający odcinek jest niezwykle szybki, ale wkrótce następuje po nim znacznie bardziej kręty i techniczny fragment utrudniony przez mroźną pogodę.", "JC_001.PNG", "Moon and Star" },
                    { 72, "Czwartym sprawdzianem Rajdu Pirelli jest najdłuższy etap rajdowych mistrzostw Wielkiej Brytanii. Punder shaw wykorzystuje fragmenty odcinka Chirdonhead i, jako taki, posiada mnóstwo długich i szybkich sekcji. Wymaga wysokiego poziomu koncentracji, gdy widocz ność jest ograniczona przez zamarzającą mżawkę.", "JC_002.PNG", "Bothwell" },
                    { 73, "Czwarty sprawdzian, Black Loch, ma coś dla każdego. Posiada dobrą, twardą nawierzchnię i samochody for muły 2 będą się rozkoszować szybkimi zjazdami, które stanowią większą część kilku początkowych kilometrów. Wielkie przepaście z prawej strony mogą sprawić, iż wielu kierowców zdejmie jednak nogę z gazu.", "JC_003.PNG", "Whitchester" },
                    { 74, "Eccles zwiastuje rozpoczęcie etapów Merse i opiera się na stosunkowo płaskim terenie na północ od rzeki Tweed. Składa się z wąskich wiejskich dróg, długich, szybkich prostych i kilku niezwykle ciasnych łuków. Wysokie żywopłoty, wymuszają precyzyjne prowadzenie samo chodu. Światło o zmierzchu również nikomu nie sprzyja.", "JC_004.PNG", "Eccles" },
                    { 75, "Etap ten demonstruje jak bardzo skomplikowane potrafią być irlandzkie drogi. Jest wąski, wyboisty i kręty oraz posiada wiele zmian kierunku jazdy, co wymaga od kie rowcy koncentracji i doskonałego prowadzenia samo chodu. W lesie Parkanaur jest wiele niebezpiecznych serii wybojów.", "JC_005.PNG", "Langton" },
                    { 76, "Kolejna legenda Manx, Tholt-y-Will, rozpoczyna się na sławnym moście i przechodzi w połowie drogi w pobliżu pubu licznie odwiedzanego przez kibiców. Dwie serpen tyny prowadzą zawodników do szybkiej części znajdującej się na szczycie góry. W górnych odcinkach często wys tępuje mgła oraz znajduje się tam wiele pastwisk.", "JC_006.PNG", "Fogo" }
                });

            migrationBuilder.InsertData(
                table: "Stages",
                columns: new[] { "Id", "Code", "IsArcade" },
                values: new object[,]
                {
                    { 1, 21, true },
                    { 2, 21, false },
                    { 3, 22, true },
                    { 4, 22, false },
                    { 5, 23, true },
                    { 6, 23, false },
                    { 7, 24, true },
                    { 8, 24, false },
                    { 9, 25, true },
                    { 10, 25, false },
                    { 11, 26, true },
                    { 12, 26, false },
                    { 13, 31, true },
                    { 14, 31, false },
                    { 15, 32, true },
                    { 16, 32, false },
                    { 17, 33, true },
                    { 18, 33, false },
                    { 19, 34, true },
                    { 20, 34, false },
                    { 21, 35, true },
                    { 22, 35, false },
                    { 23, 36, true },
                    { 24, 36, false },
                    { 25, 41, true },
                    { 26, 41, false },
                    { 27, 42, true },
                    { 28, 42, false },
                    { 29, 43, true },
                    { 30, 43, false },
                    { 31, 44, true },
                    { 32, 44, false },
                    { 33, 45, true },
                    { 34, 45, false },
                    { 35, 46, true },
                    { 36, 46, false },
                    { 37, 51, true },
                    { 38, 51, false },
                    { 39, 52, true },
                    { 40, 52, false },
                    { 41, 53, true },
                    { 42, 53, false },
                    { 43, 54, true },
                    { 44, 54, false },
                    { 45, 55, true },
                    { 46, 55, false },
                    { 47, 56, true },
                    { 48, 56, false },
                    { 49, 61, true },
                    { 50, 61, false },
                    { 51, 62, true },
                    { 52, 62, false },
                    { 53, 63, true },
                    { 54, 63, false },
                    { 55, 64, true },
                    { 56, 64, false },
                    { 57, 65, true },
                    { 58, 65, false },
                    { 59, 66, true },
                    { 60, 66, false },
                    { 61, 71, true },
                    { 62, 71, false },
                    { 63, 72, true },
                    { 64, 72, false },
                    { 65, 73, true },
                    { 66, 73, false },
                    { 67, 74, true },
                    { 68, 74, false },
                    { 69, 75, true },
                    { 70, 75, false },
                    { 71, 76, true },
                    { 72, 76, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_UserId",
                table: "Drivers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stages_Code",
                table: "Stages",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_CarId",
                table: "TimeEntries",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_DriverId",
                table: "TimeEntries",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_StageId",
                table: "TimeEntries",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_VerifyInfoId",
                table: "TimeEntries",
                column: "VerifyInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_VerifyInfos_VerifierId",
                table: "VerifyInfos",
                column: "VerifierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeEntries");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "VerifyInfos");

            migrationBuilder.DropTable(
                name: "StageWaypoints");

            migrationBuilder.DropTable(
                name: "StagesData");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
