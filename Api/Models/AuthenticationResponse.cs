namespace Api.Models;

public class AuthenticationResponse
{

#pragma warning disable CS8618
    public AuthenticationResponse()
#pragma warning restore CS8618
    {
        Status = StatusCode.Success;
        StatusText = "Success";
    }

    public StatusCode Status { get; set; }
    public string StatusText { get; set; }
    public string Token { get; set; }
}