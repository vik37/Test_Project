namespace DotNet.TestProject.IdentityInfrastructures;

public class IdentityUserDbContext : IdentityDbContext<User>
{
    public const string DB_SCHEMA  = "identity";

    public DbSet<User> User { get; set; }
    public DbSet<Gender> Gender { get; set; }

    public IdentityUserDbContext(DbContextOptions<IdentityUserDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(DB_SCHEMA);

        builder.ApplyConfiguration(new IdentityUserConfiguration());
        builder.ApplyConfiguration(new IdentityUserGenderConfiguration());
    }

}