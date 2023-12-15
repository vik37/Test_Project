
namespace DotNet.TestProject.IdentityInfrastructures.EntityConfiguration;

public class IdentityUserGenderConfiguration : IEntityTypeConfiguration<Gender>
{
    public void Configure(EntityTypeBuilder<Gender> builder)
    {
        builder.ToTable("Gender", IdentityUserDbContext.DB_SCHEMA);

        builder.HasKey(x => x.Id).IsClustered();

        builder.Property("Id").HasColumnName("Id").ValueGeneratedNever();

        builder.Property("Name").HasColumnName("Name").IsRequired();
    }
}
