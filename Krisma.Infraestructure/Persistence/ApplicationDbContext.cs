using Krisma.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Krisma.Infraestructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected ApplicationDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<Organization>(builder =>
        {
            builder.HasIndex(organization => organization.GitHubOrganizationId)
                .IsUnique();

            builder.HasIndex(organization => organization.GitHubNodeId)
                .IsUnique();

            builder.Property(organization => organization.Name)
                .HasMaxLength(200);

            builder.Property(organization => organization.GitHubNodeId)
                .HasMaxLength(100);

            builder.Property(organization => organization.TimeZoneId)
                .HasMaxLength(100);
        });
    }


    public DbSet<Developer> Developers { get; set; }
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<TechnologyDeveloper> TechnologyDevelopers { get; set; }
    public DbSet<Commit> Commits { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<PullRequest> PullRequests { get; set; }
    public DbSet<PullRequestReview> PullRequestReviews { get; set; }
    public DbSet<Repository> Repositories { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamMembership> TeamMemberships { get; set; }


}
     

