using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("authentication")]
public class AuthenticationController
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _config;

    public AuthenticationController(ILogger<AuthenticationController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    [HttpGet("token")]
    public string GetToken()
    {
        var claims = new[]
        {
            new Claim("UserId", Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1), // Token expiration time
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}