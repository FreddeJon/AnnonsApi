namespace Api.Models;

public class LoginModel
{
    [MaxLength(50)]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [MaxLength(50)]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}