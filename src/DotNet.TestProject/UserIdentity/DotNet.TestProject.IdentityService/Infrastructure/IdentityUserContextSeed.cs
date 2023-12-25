namespace DotNet.TestProject.IdentityService.Infrastructure;

/// <summary>
///  Identity User Db Context 
///  Default Data Seed.
/// </summary>
public class IdentityUserContextSeed
{
    private readonly Dictionary<string,string> _ids = new();

    #pragma warning disable
    public IdentityUserContextSeed()
    {
        _ids.Add("adminId",Guid.NewGuid().ToString());
        _ids.Add("roleId", Guid.NewGuid().ToString());
    }

    /// <summary>
    ///   Auto Seed DB Migrations to the Database 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="context"></param>
    /// <exception cref="Exception"></exception>
    public void Seed(ILogger<IdentityUserContextSeed> logger, IdentityUserDbContext context)
    {
        try
        {
            logger.LogInformation("Seed Preparation");
            if(context is not null)
            {
                var pendingMigration = context.Database.GetPendingMigrations();

                if (pendingMigration.Any())
                {
                    logger.LogInformation("Starting Migration");
                    context.Database.Migrate();
                    logger.LogInformation("Migration Was Successfully");
                }

                if (!context.Gender.Any())
                {
                    logger.LogInformation("Starting Seed Data {Gender}", typeof(Gender));
                    context.Gender.AddRange(UserIdentityGenderData());
                    context.SaveChanges();
                    logger.LogInformation("Seeding {Gender} Was Successfully", typeof(Gender));
                }
                if (!context.Roles.Any())
                {
                    logger.LogInformation("Starting Seed Data {Role}", typeof(IdentityRole));
                    context.Roles.AddRange(IdentityRoleData());
                    context.SaveChanges();
                    logger.LogInformation("Seeding {Role} Was Successfully", typeof(IdentityRole));
                }               
                if (!context.Users.Any())
                {
                    logger.LogInformation("Starting Seed Data {User}", typeof(User));
                    context.Users.AddRange(UserIdentityData());
                    context.SaveChanges();
                    logger.LogInformation("Seeding {User} Was Successfully", typeof(User));
                }
                if (!context.UserRoles.Any())
                {
                    logger.LogInformation("Starting Seed Data {UserRole}", typeof(IdentityUserRole<string>));
                    context.UserRoles.AddRange(IdentityUserRoleData());
                    context.SaveChanges();
                    logger.LogInformation("Seeding {UserRole} Was Successfully", typeof(IdentityUserRole<string>));
                }
            }
        }
        catch (SqlException ex)
        {
            logger.LogError("Seed Data to Database Failed: type {class}, Exception ({Exception}), Message {Message}", typeof(IdentityUserContextSeed),ex,ex.Message);
            throw new Exception(ex.Message);
        }
    }

    private IEnumerable<User> UserIdentityData()
    {
        var hasherPassword = new PasswordHasher<User>();
        var users = new List<User>()
        {
            new()
            {
                Id = _ids["adminId"],
                GenderId = 1,
                Firstname = "Viktor",
                Lastname = "Zafirovski",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "viktor@vik.com",
                NormalizedEmail = "viktor@vik.com",
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                Address = new Address
                {
                    Country = "Iraq",
                    State = "Mosul",
                    City = "Bagdad",
                    Street = "Some Street No.111"
                }
            }
        };
        users[0].PasswordHash = hasherPassword.HashPassword(users[0], "Admin123#");
        return users;
    }

    private IEnumerable<IdentityRole> IdentityRoleData()
        => new List<IdentityRole>()
        {
            new()
            {
                Id = _ids["roleId"],
                Name = "admin",
                NormalizedName = "admin",
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "customer",
                NormalizedName = "customer",
            }
        };

    private IEnumerable<IdentityUserRole<string>> IdentityUserRoleData()
        => new List<IdentityUserRole<string>>()
        {
            new()
            {
                UserId = _ids["adminId"],
                RoleId = _ids["roleId"]
            }
        };

    private static IEnumerable<Gender> UserIdentityGenderData()
        => new List<Gender>()
        {
            new()
            {
                Id= 1,
                Name = "Male"
            },
            new()
            {
                Id = 2,
                Name = "Female"
            }
        };
}