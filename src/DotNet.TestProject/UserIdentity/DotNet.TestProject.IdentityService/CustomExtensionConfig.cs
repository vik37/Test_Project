namespace DotNet.TestProject.IdentityService;

/// <summary>
///  Custom Configuration Class
/// </summary>
public static class CustomExtensionConfig
{
    /// <summary>
    /// Swagger Custom Configuration Extention Method
    /// </summary>
    /// <param name="services"></param>
    /// <returns>typof IServiceCollection services with additional method (SwaggerConfiguration)</returns>
    public static IServiceCollection SwaggerConfiguration(this IServiceCollection services)
        => services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Test Project - User Identity Service",
                Version = "v1",
                Description = "Service intended for User Registration and User Identity"
            });

            option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token with Bearer format like bearer [space] token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            option.IncludeXmlComments(xmlPath);
        });

    /// <summary>
    ///  CORS Custom Configuration Extention Method
    /// </summary>
    /// <param name="services"></param>
    /// <returns>typof IServiceCollection services with additional method (CorsConfiguration)</returns>
    public static IServiceCollection CorsConfiguration(this IServiceCollection services)
        => services.AddCors(options =>
        {
            options.AddPolicy("Dummy Policy", policy =>
            {
                policy.WithOrigins("http://dummy.com", "https://dummy.com")
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        });

    /// <summary>
    /// Database Custom Configuration Extention Method
    /// </summary>
    /// <param name="services"></param>
    /// <param name="dbConnectionString"></param>
    /// <returns>typof IServiceCollection services with additional method (DatabaseConfiguration)</returns>
    public static IServiceCollection DatabaseConfiguration(this IServiceCollection services,string dbConnectionString)
    {
        services.AddDbContext<IdentityUserDbContext>(options =>
        {
            var assambly = Assembly.GetExecutingAssembly().GetName().Name;

            options.UseSqlServer(dbConnectionString,op => op.MigrationsAssembly(assambly));
        });

        services.AddIdentity<User, IdentityRole>(option =>
        {
            option.Password.RequiredLength = 7;
            option.User.RequireUniqueEmail = true;
        })
         .AddEntityFrameworkStores<IdentityUserDbContext>();

        return services;
    }

    /// <summary>
    /// Seed Data by Starting the Application
    /// </summary>
    /// <param name="app"></param>
    /// <returns>typeof WebApplication app  with additional method (AddAutoSeedingInDatabase)</returns>
    public static WebApplication AddAutoSeedingInDatabase(this WebApplication app)
    {
        using(var scope = app.Services.CreateScope())
        {
            var logger = scope.ServiceProvider.GetService<ILogger<IdentityUserContextSeed>>();
            var context = scope.ServiceProvider.GetService<IdentityUserDbContext>();
            if(logger is not null || context is not null)
                new IdentityUserContextSeed().Seed(logger!, context!);
        }
        return app;
    }

    /// <summary>
    ///   JWT Token Bearer Configuration
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection JWTConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOption = configuration.GetSection(JWTOption.ConfigKey).Get<JWTOption>();

        if(jwtOption != null)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOption.Issuer,
                    ValidAudience = null,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Key))
                };
            });
        }

        return services;
    }
}