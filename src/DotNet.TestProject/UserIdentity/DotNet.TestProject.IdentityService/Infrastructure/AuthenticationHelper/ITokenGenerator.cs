namespace DotNet.TestProject.IdentityService.Infrastructure.AuthenticationHelper;

public interface ITokenGenerator
{
    string Generator(UserClaims userClaims);
}