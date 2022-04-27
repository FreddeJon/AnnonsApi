namespace Api.Models;

public class AuthenticationResponse
{

    public AuthenticationResponse()
    {
        Status = StatusCode.Success;
        StatusText = "Success";
    }

    public StatusCode Status { get; set; }
    public string StatusText { get; set; }
    public string Token { get; set; }
}