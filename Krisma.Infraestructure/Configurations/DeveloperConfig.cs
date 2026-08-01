using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Krisma.Domain.Entities;
using Krisma.Domain.ValueObjects;

namespace Krisma.Infraestructure.Configurations;

internal class DeveloperConfig : IEntityTypeConfiguration<Developer>
{
    public void Configure(EntityTypeBuilder<Developer> builder)
    {
        

        builder.HasKey(prop => prop.Id);

        builder.Property(prop => prop.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(prop => prop.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(prop => prop.LastName)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(prop => prop.GitHubLogin)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(prop => prop.GitHubLogin)
            .IsUnique();

        builder.Property(prop => prop.Email)
            .IsRequired()
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).Value);

        builder.Property(prop => prop.Seniority)
            .IsRequired();

        builder.Property(prop => prop.HireDate)
            .IsRequired();

        builder.Property(prop => prop.Position)
            .IsRequired();

        builder.Property(prop => prop.Department)
            .IsRequired();

        builder.Property(prop => prop.ManagerId)
            .IsRequired(false);
    }
}