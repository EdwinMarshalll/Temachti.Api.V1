namespace Temachti.Api.DTOs;

public class DTOAuthenticationRequest
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}