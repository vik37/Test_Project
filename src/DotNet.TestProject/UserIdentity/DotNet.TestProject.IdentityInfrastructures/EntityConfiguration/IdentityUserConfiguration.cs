namespace DotNet.TestProject.IdentityInfrastructures.EntityConfiguration;

public class IdentityUserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", IdentityUserDbContext.DB_SCHEMA);
        
        builder.Property<string>("Firstname").HasColumnName("Firstname").IsRequired();

        builder.Property<string>("Lastname").HasColumnName("Lastname").IsRequired();

        builder.Property<int?>("GenderId").HasColumnName("GenderId").IsRequired(false);

        builder.OwnsOne(u => u.Address);

        builder.HasOne(u => u.Gender)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.GenderId);
    }
}