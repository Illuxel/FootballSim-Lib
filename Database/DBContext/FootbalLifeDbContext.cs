using System;
using System.Collections.Generic;
using FootBalLife.GameDB.Models;
using Microsoft.EntityFrameworkCore;

namespace FootBalLife.GameDB.Context;

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
            entity.HasKey(e => e.PersonId);

            entity.ToTable("Agent");

            entity.HasOne(d => d.Person).WithOne(p => p.Agent)
                .HasForeignKey<Agent>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.PersonId);

            entity.ToTable("Coach");

            entity.HasOne(d => d.Person).WithOne(p => p.Coach)
                .HasForeignKey<Coach>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.ToTable("Contract");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SeasonFrom).HasColumnType("TEXT");
            entity.Property(e => e.SeasonTo).HasColumnType("TEXT");

            entity.HasOne(d => d.Person).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Team).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasKey(e => e.PersonId);

            entity.ToTable("Director");

            entity.HasOne(d => d.Person).WithOne(p => p.Director)
                .HasForeignKey<Director>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<League>(entity =>
        {
            entity.ToTable("League");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CurrentRating).HasDefaultValueSql("0");

            entity.HasOne(d => d.Country).WithMany(p => p.Leagues).HasForeignKey(d => d.CountryId);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.HomeTeam, e.GuestTeam, e.Season });

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.Team1Navigation).WithMany(p => p.MatchTeam1Navigations)
                .HasForeignKey(d => d.HomeTeam)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Team2Navigation).WithMany(p => p.MatchTeam2Navigations)
                .HasForeignKey(d => d.GuestTeam)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<NationalResultTable>(entity =>
        {
            entity.HasKey(e => new { e.Season, e.TeamId });

            entity.ToTable("NationalResultTable");

            entity.HasOne(d => d.Team).WithMany(p => p.NationalResultTables)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.Country).WithMany(p => p.People)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.CurrentRole).WithMany(p => p.People).HasForeignKey(d => d.CurrentRoleId);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PersonId);

            entity.ToTable("Player");

            entity.HasOne(d => d.Contract).WithMany(p => p.Players).HasForeignKey(d => d.ContractId);

            entity.HasOne(d => d.Person).WithOne(p => p.Player)
                .HasForeignKey<Player>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PositionNavigation).WithMany(p => p.Players).HasForeignKey(d => d.PositionId);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsNpc).HasColumnName("isNPC");
        });

        modelBuilder.Entity<Scout>(entity =>
        {
            entity.HasKey(e => e.PersonId);

            entity.ToTable("Scout");

            entity.HasOne(d => d.Person).WithOne(p => p.Scout)
                .HasForeignKey<Scout>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("Team");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsNationalTeam).HasDefaultValueSql("0");
            entity.Property(e => e.Strategy).HasDefaultValueSql("0");

            entity.HasOne(d => d.League).WithMany(p => p.Teams).HasForeignKey(d => d.LeagueId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
