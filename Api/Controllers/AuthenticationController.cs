using Api.Application.Models;
using Api.Services;

namespace Api.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthenticationController(IAuthenticationService authenticationService, UserManager<IdentityUser> userManager)
    {
        _authenticationService = authenticationService;
        _userManager = userManager;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);



        var response = await _authenticationService.ValidateUser(model.Email, model.Password);

        if (response.Status == Application.Models.StatusCode.Unauthorized)
        {
            return Unauthorized(response.StatusText);
        }

        
        return Ok(new
        {
            Status = response.StatusText,
            Token = response.Token
        });

    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

            var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Email already in use!" });
        }
       


        IdentityUser user = new()
        {
            Email = model.Email,
            UserName = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed!", Errors = result.Errors.Select(x => x.Description).ToList() });
        }

        return Created("Success", new { Status = "Success", Title = "User created successfully!", User = new { user.UserName, user.Email, user.Id } });
    }
}