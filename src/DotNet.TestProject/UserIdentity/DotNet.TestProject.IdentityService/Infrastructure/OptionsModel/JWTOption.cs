namespace DotNet.TestProject.IdentityService.Infrastructure.OptionsModel;

/// <summary>
///  JWT Options Model Intended for Mappingthe Values 
///  from Connection String (Key:JWT)
/// </summary>
public class JWTOption
{
    #pragma warning disable
    public const string ConfigKey = "JWT";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
}