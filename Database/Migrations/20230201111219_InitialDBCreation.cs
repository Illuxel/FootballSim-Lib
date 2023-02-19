using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialDBCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Location = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    isNPC = table.Column<long>(type: "INTEGER", nullable: true),
                    Icon = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "League",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentRating = table.Column<long>(type: "INTEGER", nullable: true, defaultValueSql: "0"),
                    CountryID = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_League", x => x.ID);
                    table.ForeignKey(
                        name: "FK_League_Country_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Birthday = table.Column<byte[]>(type: "DATE", nullable: false),
                    CurrentRoleID = table.Column<long>(type: "INTEGER", nullable: true),
                    CountryID = table.Column<long>(type: "INTEGER", nullable: false),
                    Icon = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Person_Country_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Person_Role_CurrentRoleID",
                        column: x => x.CurrentRoleID,
                        principalTable: "Role",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LeagueID = table.Column<long>(type: "INTEGER", nullable: true),
                    SportsDirectorID = table.Column<long>(type: "INTEGER", nullable: true),
                    CoachID = table.Column<long>(type: "INTEGER", nullable: true),
                    AgentID = table.Column<long>(type: "INTEGER", nullable: true),
                    IsNationalTeam = table.Column<long>(type: "INTEGER", nullable: true, defaultValueSql: "0"),
                    Strategy = table.Column<long>(type: "INTEGER", nullable: true, defaultValueSql: "0"),
                    BaseColor = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Team_League_LeagueID",
                        column: x => x.LeagueID,
                        principalTable: "League",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Agent",
                columns: table => new
                {
                    PesonID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agent", x => x.PesonID);
                    table.ForeignKey(
                        name: "FK_Agent_Person_PesonID",
                        column: x => x.PesonID,
                        principalTable: "Person",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Coach",
                columns: table => new
                {
                    PesonID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coach", x => x.PesonID);
                    table.ForeignKey(
                        name: "FK_Coach_Person_PesonID",
                        column: x => x.PesonID,
                        principalTable: "Person",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Director",
                columns: table => new
                {
                    PesonID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Director", x => x.PesonID);
                    table.ForeignKey(
                        name: "FK_Director_Person_PesonID",
                        column: x => x.PesonID,
                        principalTable: "Person",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Scout",
                columns: table => new
                {
                    PesonID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scout", x => x.PesonID);
                    table.ForeignKey(
                        name: "FK_Scout_Person_PesonID",
                        column: x => x.PesonID,
                        principalTable: "Person",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    SeasonFrom = table.Column<byte[]>(type: "DATE", nullable: false),
                    SeasonTo = table.Column<byte[]>(type: "DATE", nullable: false),
                    TeamID = table.Column<string>(type: "TEXT", nullable: false),
                    PersonID = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Contract_Person_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Person",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Contract_Team_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Team",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    HomeTeam = table.Column<string>(type: "TEXT", nullable: false),
                    GuestTeam = table.Column<string>(type: "TEXT", nullable: false),
                    Season = table.Column<string>(type: "TEXT", nullable: false),
                    WeekNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    HomeTeamGoals = table.Column<long>(type: "INTEGER", nullable: false),
                    GuestTeamGoals = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => new { x.ID, x.HomeTeam, x.GuestTeam, x.Season });
                    table.ForeignKey(
                        name: "FK_Matches_Team_HomeTeam",
                        column: x => x.HomeTeam,
                        principalTable: "Team",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Matches_Team_GuestTeam",
                        column: x => x.GuestTeam,
                        principalTable: "Team",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "NationalResultTable",
                columns: table => new
                {
                    Season = table.Column<string>(type: "TEXT", nullable: false),
                    TeamID = table.Column<string>(type: "TEXT", nullable: false),
                    Wins = table.Column<long>(type: "INTEGER", nullable: false),
                    Draws = table.Column<long>(type: "INTEGER", nullable: false),
                    Loses = table.Column<long>(type: "INTEGER", nullable: false),
                    ScoredGoals = table.Column<long>(type: "INTEGER", nullable: false),
                    MissedGoals = table.Column<long>(type: "INTEGER", nullable: false),
                    TotalPosition = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NationalResultTable", x => new { x.Season, x.TeamID });
                    table.ForeignKey(
                        name: "FK_NationalResultTable_Team_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Team",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PersonID = table.Column<string>(type: "TEXT", nullable: false),
                    PositionID = table.Column<long>(type: "INTEGER", nullable: true),
                    ContractID = table.Column<string>(type: "TEXT", nullable: true),
                    Speed = table.Column<long>(type: "INTEGER", nullable: false),
                    KickCount = table.Column<long>(type: "INTEGER", nullable: false),
                    Endurance = table.Column<long>(type: "INTEGER", nullable: false),
                    Strike = table.Column<long>(type: "INTEGER", nullable: true),
                    Physics = table.Column<long>(type: "INTEGER", nullable: false),
                    Position = table.Column<long>(type: "INTEGER", nullable: true),
                    Technique = table.Column<long>(type: "INTEGER", nullable: false),
                    Passing = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.PersonID);
                    table.ForeignKey(
                        name: "FK_Player_Contract_ContractID",
                        column: x => x.ContractID,
                        principalTable: "Contract",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Player_Person_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Person",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Player_Position_PositionID",
                        column: x => x.PositionID,
                        principalTable: "Position",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_PersonID",
                table: "Contract",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_TeamID",
                table: "Contract",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_League_CountryID",
                table: "League",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeam",
                table: "Matches",
                column: "HomeTeam");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_GuestTeam",
                table: "Matches",
                column: "GuestTeam");

            migrationBuilder.CreateIndex(
                name: "IX_NationalResultTable_TeamID",
                table: "NationalResultTable",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_CountryID",
                table: "Person",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_CurrentRoleID",
                table: "Person",
                column: "CurrentRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_ContractID",
                table: "Player",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_PositionID",
                table: "Player",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_LeagueID",
                table: "Team",
                column: "LeagueID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agent");

            migrationBuilder.DropTable(
                name: "Coach");

            migrationBuilder.DropTable(
                name: "Director");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "NationalResultTable");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Scout");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "League");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
