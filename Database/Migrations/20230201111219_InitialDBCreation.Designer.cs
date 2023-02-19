﻿// <auto-generated />
using System;
using FootBalLife.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(FootbalLifeDbContext))]
    [Migration("20230201111219_InitialDBCreation")]
    partial class InitialDBCreation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("FootBalLife.Database.Models.Agent", b =>
                {
                    b.Property<string>("PesonID")
                        .HasColumnType("TEXT");

                    b.HasKey("PesonID");

                    b.ToTable("Agent", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Coach", b =>
                {
                    b.Property<string>("PesonID")
                        .HasColumnType("TEXT");

                    b.HasKey("PesonID");

                    b.ToTable("Coach", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Contract", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT")
                        .HasColumnName("ID");

                    b.Property<string>("PersonID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("SeasonFrom")
                        .IsRequired()
                        .HasColumnType("DATE");

                    b.Property<byte[]>("SeasonTo")
                        .IsRequired()
                        .HasColumnType("DATE");

                    b.Property<string>("TeamID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("PersonID");

                    b.HasIndex("TeamID");

                    b.ToTable("Contract", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Country", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ID");

                    b.Property<byte[]>("Icon")
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Country", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Director", b =>
                {
                    b.Property<string>("PesonID")
                        .HasColumnType("TEXT");

                    b.HasKey("PesonID");

                    b.ToTable("Director", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.League", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ID");

                    b.Property<long?>("CountryID")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("CurrentRating")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CountryID");

                    b.ToTable("League", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Match", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT")
                        .HasColumnName("ID");

                    b.Property<string>("HomeTeam")
                        .HasColumnType("TEXT");

                    b.Property<string>("GuestTeam")
                        .HasColumnType("TEXT");

                    b.Property<string>("Season")
                        .HasColumnType("TEXT");

                    b.Property<long>("HomeTeamGoals")
                        .HasColumnType("INTEGER");

                    b.Property<long>("GuestTeamGoals")
                        .HasColumnType("INTEGER");

                    b.Property<long>("WeekNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID", "HomeTeam", "GuestTeam", "Season");

                    b.HasIndex("HomeTeam");

                    b.HasIndex("GuestTeam");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.NationalResultTable", b =>
                {
                    b.Property<string>("Season")
                        .HasColumnType("TEXT");

                    b.Property<string>("TeamID")
                        .HasColumnType("TEXT");

                    b.Property<long>("Draws")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Loses")
                        .HasColumnType("INTEGER");

                    b.Property<long>("MissedGoals")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ScoredGoals")
                        .HasColumnType("INTEGER");

                    b.Property<long>("TotalPosition")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Wins")
                        .HasColumnType("INTEGER");

                    b.HasKey("Season", "TeamID");

                    b.HasIndex("TeamID");

                    b.ToTable("NationalResultTable", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Person", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT")
                        .HasColumnName("ID");

                    b.Property<byte[]>("Birthday")
                        .IsRequired()
                        .HasColumnType("DATE");

                    b.Property<long>("CountryID")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("CurrentRoleID")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Icon")
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CountryID");

                    b.HasIndex("CurrentRoleID");

                    b.ToTable("Person", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Player", b =>
                {
                    b.Property<string>("PersonID")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContractID")
                        .HasColumnType("TEXT");

                    b.Property<long>("Endurance")
                        .HasColumnType("INTEGER");

                    b.Property<long>("KickCount")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Passing")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Physics")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Position")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("PositionID")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Strike")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Speed")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Technique")
                        .HasColumnType("INTEGER");

                    b.HasKey("PersonID");

                    b.HasIndex("ContractID");

                    b.HasIndex("PositionID");

                    b.ToTable("Player", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Position", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ID");

                    b.Property<long>("Location")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Position", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Role", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ID");

                    b.Property<byte[]>("Icon")
                        .HasColumnType("BLOB");

                    b.Property<long?>("IsNpc")
                        .HasColumnType("INTEGER")
                        .HasColumnName("isNPC");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Scout", b =>
                {
                    b.Property<string>("PesonID")
                        .HasColumnType("TEXT");

                    b.HasKey("PesonID");

                    b.ToTable("Scout", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Team", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT")
                        .HasColumnName("ID");

                    b.Property<long?>("AgentID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("BaseColor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("CoachID")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("IsNationalTeam")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("0");

                    b.Property<long?>("LeagueID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("SportsDirectorID")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Strategy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("0");

                    b.HasKey("ID");

                    b.HasIndex("LeagueID");

                    b.ToTable("Team", (string)null);
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Agent", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.Person", "Peson")
                        .WithOne("Agent")
                        .HasForeignKey("FootBalLife.Database.Models.Agent", "PesonID")
                        .IsRequired();

                    b.Navigation("Peson");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Coach", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.Person", "Peson")
                        .WithOne("Coach")
                        .HasForeignKey("FootBalLife.Database.Models.Coach", "PesonID")
                        .IsRequired();

                    b.Navigation("Peson");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Contract", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.Person", "Person")
                        .WithMany("Contracts")
                        .HasForeignKey("PersonID")
                        .IsRequired();

                    b.HasOne("FootBalLife.Database.Models.Team", "Team")
                        .WithMany("Contracts")
                        .HasForeignKey("TeamID")
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Director", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.Person", "Peson")
                        .WithOne("Director")
                        .HasForeignKey("FootBalLife.Database.Models.Director", "PesonID")
                        .IsRequired();

                    b.Navigation("Peson");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.League", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.Country", "Country")
                        .WithMany("Leagues")
                        .HasForeignKey("CountryID");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Match", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.Team", "HomeTeamNavigation")
                        .WithMany("MatchHomeTeamNavigations")
                        .HasForeignKey("HomeTeam")
                        .IsRequired();

                    b.HasOne("FootBalLife.Database.Models.Team", "GuestTeamNavigation")
                        .WithMany("MatchGuestTeamNavigations")
                        .HasForeignKey("GuestTeam")
                        .IsRequired();

                    b.Navigation("HomeTeamNavigation");

                    b.Navigation("GuestTeamNavigation");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.NationalResultTable", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.Team", "Team")
                        .WithMany("NationalResultTables")
                        .HasForeignKey("TeamID")
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Person", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.Country", "Country")
                        .WithMany("People")
                        .HasForeignKey("CountryID")
                        .IsRequired();

                    b.HasOne("FootBalLife.Database.Models.Role", "CurrentRole")
                        .WithMany("People")
                        .HasForeignKey("CurrentRoleID");

                    b.Navigation("Country");

                    b.Navigation("CurrentRole");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Player", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.Contract", "Contract")
                        .WithMany("Players")
                        .HasForeignKey("ContractID");

                    b.HasOne("FootBalLife.Database.Models.Person", "Person")
                        .WithOne("Player")
                        .HasForeignKey("FootBalLife.Database.Models.Player", "PersonID")
                        .IsRequired();

                    b.HasOne("FootBalLife.Database.Models.Position", "PositionNavigation")
                        .WithMany("Players")
                        .HasForeignKey("PositionID");

                    b.Navigation("Contract");

                    b.Navigation("Person");

                    b.Navigation("PositionNavigation");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Scout", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.Person", "Peson")
                        .WithOne("Scout")
                        .HasForeignKey("FootBalLife.Database.Models.Scout", "PesonID")
                        .IsRequired();

                    b.Navigation("Peson");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Team", b =>
                {
                    b.HasOne("FootBalLife.Database.Models.League", "League")
                        .WithMany("Teams")
                        .HasForeignKey("LeagueID");

                    b.Navigation("League");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Contract", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Country", b =>
                {
                    b.Navigation("Leagues");

                    b.Navigation("People");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.League", b =>
                {
                    b.Navigation("Teams");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Person", b =>
                {
                    b.Navigation("Agent");

                    b.Navigation("Coach");

                    b.Navigation("Contracts");

                    b.Navigation("Director");

                    b.Navigation("Player");

                    b.Navigation("Scout");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Position", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Role", b =>
                {
                    b.Navigation("People");
                });

            modelBuilder.Entity("FootBalLife.Database.Models.Team", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("MatchHomeTeamNavigations");

                    b.Navigation("MatchGuestTeamNavigations");

                    b.Navigation("NationalResultTables");
                });
#pragma warning restore 612, 618
        }
    }
}
