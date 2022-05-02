using Api.Application.Models;

namespace Api.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }



    public async Task<AuthenticationResponse> ValidateUser(string email, string password)
    {
        var response = new AuthenticationResponse();
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            response.Status = StatusCode.Unauthorized;
            response.StatusText = "Email or password is incorrect";
            return response;
        }

        var correctPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!correctPassword)
        {
            response.Status = StatusCode.Unauthorized;
            response.StatusText = "Email or password is incorrect";
            return response;
        }

        var claims = GetClaimsForUser(user);
        var generatedSecurityToken = CreateToken(claims);


        response.Token = new JwtSecurityTokenHandler().WriteToken(generatedSecurityToken);
        return response;
    }

    public JwtSecurityToken CreateToken(List<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:Secret"]));
        _ = int.TryParse(_configuration["JwtOptions:TokenValidityInMinutes"], out int tokenValidityInMinutes);


        return new JwtSecurityToken(
            issuer: _configuration["JwtOptions:ValidIssuer"],
            audience: _configuration["JwtOptions:ValidAudience"],
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        ); ;
    }

    public List<Claim> GetClaimsForUser(IdentityUser user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        return authClaims;
    }
}