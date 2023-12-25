namespace DotNet.TestProject.IdentityService.Infrastructure.AuthenticationHelper;

#pragma warning disable
public interface ITokenGenerator
{
    #pragma warning disable
    string Generator(UserClaims userClaims);
}