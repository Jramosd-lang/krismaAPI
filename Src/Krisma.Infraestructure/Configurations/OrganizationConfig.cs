using Krisma.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Krisma.Infraestructure.Configurations;

internal class OrganizationConfig : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(prop => prop.Id);

        builder.Property(prop => prop.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(prop => prop.GitHubOrganizationId)
            .IsRequired();

        builder.HasIndex(prop => prop.GitHubOrganizationId)
            .IsUnique();

        builder.Property(prop => prop.GitHubNodeId)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(prop => prop.GitHubNodeId)
            .IsUnique();

        builder.Property(prop => prop.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(prop => prop.Name)
            .IsUnique();

        builder.Property(prop => prop.TimeZoneId)
            .IsRequired()
            .HasMaxLength(50);
    }
}