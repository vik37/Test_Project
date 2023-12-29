
Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

try
{
    Log.Information("Identity User Application was Started");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context,logConf) => logConf.WriteTo.Console()
                                                            .ReadFrom.Configuration(context.Configuration));

    builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    });

    IServiceCollection services = builder.Services;
    IConfiguration configuration = builder.Configuration;

    services.Configure<JWTOption>(configuration.GetSection(JWTOption.ConfigKey));

    // Add services to the container.
    services.AddControllers()
    .AddNewtonsoftJson(opt =>
        {
        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

    // custom config
    services.SwaggerConfiguration()
            .CorsConfiguration()
            .DatabaseConfiguration(configuration["IdentityUserDbConnection"] ?? throw new Exception("Missing Database Connection String"))
            .JWTConfiguration(configuration);

    services.AddMediatR(conf =>
    {
        conf.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());       
    });
    services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    services.AddScoped<ITokenGenerator, TokenGenerator>();

    services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
    
    
    services.AddAutoMapper(typeof(Program));

    services.AddTransient<IUserQuery, UserQuery>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(sw =>
        {
            sw.SwaggerEndpoint("/swagger/v1/swagger.json", "User - Identity Service");
        });
    }

    //app.ExceptionHandlerConfiguration();
    app.UseMiddleware<GlobalExceptionMiddleware>();

    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    
    app.UseAuthentication();
    app.UseAuthorization();

   


    app.MapControllers();

    app.AddAutoSeedingInDatabase();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Something bad happend");
}
finally
{
    Log.CloseAndFlush();
}