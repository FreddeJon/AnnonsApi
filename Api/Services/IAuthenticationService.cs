namespace Api.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> ValidateUser(string email, string password);
    JwtSecurityToken CreateToken(List<Claim> claims);
    List<Claim> GetClaimsForUser(IdentityUser user);
}