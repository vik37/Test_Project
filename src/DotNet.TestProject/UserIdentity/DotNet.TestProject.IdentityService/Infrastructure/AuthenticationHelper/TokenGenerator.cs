using System.Security.Claims;

namespace DotNet.TestProject.IdentityService.Infrastructure.AuthenticationHelper;

public class TokenGenerator : ITokenGenerator
{
    public string Generator(UserClaims userClaims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,userClaims.Id),
            new Claim(ClaimTypes.GivenName,$"{userClaims.Firstname} {userClaims.Lastname}"),
            new Claim(ClaimTypes.Email,userClaims.Email),
            new Claim(ClaimTypes.Role,userClaims.Role)
        };

        var secToken = new JwtSecurityToken(
                issuer: JwtOptions.Issuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(secToken);
    }
}