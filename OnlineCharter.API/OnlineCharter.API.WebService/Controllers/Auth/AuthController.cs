using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;

namespace OnlineCharter.API.WebService.Controllers.Auth
{
    [Route("auth")]
    public class AuthController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("token")]
        public IActionResult Auth(string tokenId)
        {
                var payload = GoogleJsonWebSignature.ValidateAsync(
                    tokenId, 
                    new GoogleJsonWebSignature.ValidationSettings()).Result;

                var user = _authService.Authenticate(payload.Name, payload.Email);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("secret"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    string.Empty,
                    string.Empty,
                    claims,
                    expires: DateTime.Now.AddSeconds(55 * 60),
                    signingCredentials: creds);

                return new OkObjectResult(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
        }
    }
}
