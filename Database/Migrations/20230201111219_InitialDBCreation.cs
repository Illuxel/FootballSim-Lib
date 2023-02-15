using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameDB.Migrations
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
                    CountryId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_League", x => x.ID);
                    table.ForeignKey(
                        name: "FK_League_Country_CountryId",
                        column: x => x.CountryId,
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
                    CurrentRoleId = table.Column<long>(type: "INTEGER", nullable: true),
                    CountryId = table.Column<long>(type: "INTEGER", nullable: false),
                    Icon = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Person_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Person_Role_CurrentRoleId",
                        column: x => x.CurrentRoleId,
                        principalTable: "Role",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LeagueId = table.Column<long>(type: "INTEGER", nullable: true),
                    SportsDirectorId = table.Column<long>(type: "INTEGER", nullable: true),
                    CoachId = table.Column<long>(type: "INTEGER", nullable: true),
                    AgentId = table.Column<long>(type: "INTEGER", nullable: true),
                    IsNationalTeam = table.Column<long>(type: "INTEGER", nullable: true, defaultValueSql: "0"),
                    Strategy = table.Column<long>(type: "INTEGER", nullable: true, defaultValueSql: "0"),
                    BaseColor = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Team_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Agent",
                columns: table => new
                {
                    PesonId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agent", x => x.PesonId);
                    table.ForeignKey(
                        name: "FK_Agent_Person_PesonId",
                        column: x => x.PesonId,
                        principalTable: "Person",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Coach",
                columns: table => new
                {
                    PesonId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coach", x => x.PesonId);
                    table.ForeignKey(
                        name: "FK_Coach_Person_PesonId",
                        column: x => x.PesonId,
                        principalTable: "Person",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Director",
                columns: table => new
                {
                    PesonId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Director", x => x.PesonId);
                    table.ForeignKey(
                        name: "FK_Director_Person_PesonId",
                        column: x => x.PesonId,
                        principalTable: "Person",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Scout",
                columns: table => new
                {
                    PesonId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scout", x => x.PesonId);
                    table.ForeignKey(
                        name: "FK_Scout_Person_PesonId",
                        column: x => x.PesonId,
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
                    TeamId = table.Column<string>(type: "TEXT", nullable: false),
                    PersonId = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Contract_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Contract_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    Team1 = table.Column<string>(type: "TEXT", nullable: false),
                    Team2 = table.Column<string>(type: "TEXT", nullable: false),
                    Season = table.Column<string>(type: "TEXT", nullable: false),
                    WeekNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    Team1Goals = table.Column<long>(type: "INTEGER", nullable: false),
                    Team2Goals = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => new { x.ID, x.Team1, x.Team2, x.Season });
                    table.ForeignKey(
                        name: "FK_Matches_Team_Team1",
                        column: x => x.Team1,
                        principalTable: "Team",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Matches_Team_Team2",
                        column: x => x.Team2,
                        principalTable: "Team",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "NationalResultTable",
                columns: table => new
                {
                    Season = table.Column<string>(type: "TEXT", nullable: false),
                    TeamId = table.Column<string>(type: "TEXT", nullable: false),
                    Wins = table.Column<long>(type: "INTEGER", nullable: false),
                    Draws = table.Column<long>(type: "INTEGER", nullable: false),
                    Loses = table.Column<long>(type: "INTEGER", nullable: false),
                    ScoredGoals = table.Column<long>(type: "INTEGER", nullable: false),
                    MissedGoals = table.Column<long>(type: "INTEGER", nullable: false),
                    TotalPosition = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NationalResultTable", x => new { x.Season, x.TeamId });
                    table.ForeignKey(
                        name: "FK_NationalResultTable_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PersonId = table.Column<string>(type: "TEXT", nullable: false),
                    PositionId = table.Column<long>(type: "INTEGER", nullable: true),
                    ContractId = table.Column<string>(type: "TEXT", nullable: true),
                    Speed = table.Column<long>(type: "INTEGER", nullable: false),
                    KickCount = table.Column<long>(type: "INTEGER", nullable: false),
                    Endurance = table.Column<long>(type: "INTEGER", nullable: false),
                    Reflex = table.Column<long>(type: "INTEGER", nullable: true),
                    Physics = table.Column<long>(type: "INTEGER", nullable: false),
                    Position = table.Column<long>(type: "INTEGER", nullable: true),
                    Technique = table.Column<long>(type: "INTEGER", nullable: false),
                    Passing = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_Player_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Player_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Player_Position_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_PersonId",
                table: "Contract",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_TeamId",
                table: "Contract",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_League_CountryId",
                table: "League",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Team1",
                table: "Matches",
                column: "Team1");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Team2",
                table: "Matches",
                column: "Team2");

            migrationBuilder.CreateIndex(
                name: "IX_NationalResultTable_TeamId",
                table: "NationalResultTable",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_CountryId",
                table: "Person",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_CurrentRoleId",
                table: "Person",
                column: "CurrentRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_ContractId",
                table: "Player",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_PositionId",
                table: "Player",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_LeagueId",
                table: "Team",
                column: "LeagueId");
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
