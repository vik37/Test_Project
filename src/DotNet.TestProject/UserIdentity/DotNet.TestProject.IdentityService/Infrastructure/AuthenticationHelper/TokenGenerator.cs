namespace DotNet.TestProject.IdentityService.Infrastructure.AuthenticationHelper;

#pragma warning disable
public class TokenGenerator : ITokenGenerator
{
    private readonly JWTOption _jwtOption;

    #pragma warning disable
    public TokenGenerator(IOptions<JWTOption> options)
    {
        _jwtOption = options.Value;
    }

    /// <summary>
    ///  Generate JWT Token String for User Login
    /// </summary>
    /// <param name="userClaims"></param>
    /// <returns></returns>
    public string Generator(UserClaims userClaims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,userClaims.Id),
            new Claim(ClaimTypes.GivenName,$"{userClaims.Firstname} {userClaims.Lastname}"),
            new Claim(ClaimTypes.Email,userClaims.Email),
            new Claim(ClaimTypes.Role,userClaims.Role)
        };

        var secToken = new JwtSecurityToken(
                issuer: _jwtOption.Issuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(secToken);
    }
}