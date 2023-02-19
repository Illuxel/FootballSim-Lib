using System;
using System.Collections.Generic;
using FootBalLife.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FootBalLife.Database.Context;

public partial class FootbalLifeDbContext : DbContext
{
    public FootbalLifeDbContext()
    {
    }

    public FootbalLifeDbContext(DbContextOptions<FootbalLifeDbContext> options)
        : base(options)
    {
    }

    internal virtual DbSet<Agent> Agents { get; set; }

    internal virtual DbSet<Coach> Coaches { get; set; }

    internal virtual DbSet<Contract> Contracts { get; set; }

    internal virtual DbSet<Country> Countries { get; set; }

    internal virtual DbSet<Director> Directors { get; set; }

    internal virtual DbSet<League> Leagues { get; set; }

    internal virtual DbSet<Match> Matches { get; set; }

    internal virtual DbSet<NationalResultTable> NationalResultTables { get; set; }

    internal virtual DbSet<Person> People { get; set; }

    internal virtual DbSet<Player> Players { get; set; }

    internal virtual DbSet<Position> Positions { get; set; }

    internal virtual DbSet<Role> Roles { get; set; }

    internal virtual DbSet<Scout> Scouts { get; set; }

    internal virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("DataSource=.\\DataBase\\FootbalLifeDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.PersonID);

            entity.ToTable("Agent");

            entity.HasOne(d => d.Person).WithOne(p => p.Agent)
                .HasForeignKey<Agent>(d => d.PersonID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.PersonID);

            entity.ToTable("Coach");

            entity.HasOne(d => d.Person).WithOne(p => p.Coach)
                .HasForeignKey<Coach>(d => d.PersonID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.ToTable("Contract");

            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.SeasonFrom).HasColumnType("TEXT");
            entity.Property(e => e.SeasonTo).HasColumnType("TEXT");

            entity.HasOne(d => d.Person).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.PersonID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Team).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.TeamID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.ID).HasColumnName("ID");
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasKey(e => e.PersonID);

            entity.ToTable("Director");

            entity.HasOne(d => d.Person).WithOne(p => p.Director)
                .HasForeignKey<Director>(d => d.PersonID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<League>(entity =>
        {
            entity.ToTable("League");

            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.CurrentRating).HasDefaultValueSql("0");

            entity.HasOne(d => d.Country).WithMany(p => p.Leagues).HasForeignKey(d => d.CountryID);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => new { e.ID, e.HomeTeam, e.GuestTeam, e.Season });

            entity.Property(e => e.ID).HasColumnName("ID");

            entity.HasOne(d => d.HomeTeamNavigation).WithMany(p => p.MatchHomeTeamNavigations)
                .HasForeignKey(d => d.HomeTeam)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.GuestTeamNavigation).WithMany(p => p.MatchGuestTeamNavigations)
                .HasForeignKey(d => d.GuestTeam)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<NationalResultTable>(entity =>
        {
            entity.HasKey(e => new { e.Season, e.TeamID });

            entity.ToTable("NationalResultTable");

            entity.HasOne(d => d.Team).WithMany(p => p.NationalResultTables)
                .HasForeignKey(d => d.TeamID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.Property(e => e.ID).HasColumnName("ID");

            entity.HasOne(d => d.Country).WithMany(p => p.People)
                .HasForeignKey(d => d.CountryID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.CurrentRole).WithMany(p => p.People).HasForeignKey(d => d.CurrentRoleID);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PersonID);

            entity.ToTable("Player");

            entity.HasOne(d => d.Contract).WithMany(p => p.Players).HasForeignKey(d => d.ContractID);

            entity.HasOne(d => d.Person).WithOne(p => p.Player)
                .HasForeignKey<Player>(d => d.PersonID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PositionNavigation).WithMany(p => p.Players).HasForeignKey(d => d.PositionID);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.Property(e => e.ID).HasColumnName("ID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.IsNpc).HasColumnName("isNPC");
        });

        modelBuilder.Entity<Scout>(entity =>
        {
            entity.HasKey(e => e.PersonID);

            entity.ToTable("Scout");

            entity.HasOne(d => d.Person).WithOne(p => p.Scout)
                .HasForeignKey<Scout>(d => d.PersonID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("Team");

            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.IsNationalTeam).HasDefaultValueSql("0");
            entity.Property(e => e.Strategy).HasDefaultValueSql("0");

            entity.HasOne(d => d.League).WithMany(p => p.Teams).HasForeignKey(d => d.LeagueID);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
