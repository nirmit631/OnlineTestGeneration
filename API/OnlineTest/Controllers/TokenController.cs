using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OnlineTest.Model;
using OnlineTest.Services.DTO;
using OnlineTest.Services.Interface;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRTokenService _rTokenService;
        private readonly IOptions<JWTConfigDTO> _jwtConfig;
        public TokenController(IUserService userService, IRTokenService rTokenService, IOptions<JWTConfigDTO> jwtConfig)
        {
            _userService = userService;
            _rTokenService = rTokenService;
            _jwtConfig = jwtConfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [Route("Auth")]
        public void Auth([FromBody] TokenDTO parameters)
        {
            switch (parameters.Grant_Type)
            {
                case "password" when string.IsNullOrEmpty(parameters.Username):
                    throw new Exception("Enter Username.");
                case "password" when string.IsNullOrEmpty(parameters.Username) && string.IsNullOrEmpty(parameters.Password):
                    throw new Exception("Enter Username & Password.");
                case "password" when !string.IsNullOrEmpty(parameters.Username) && !string.IsNullOrEmpty(parameters.Password):
                    Login(parameters);
                    break;
                case "password" when !string.IsNullOrEmpty(parameters.Username) && parameters.Password == "":
                    throw new Exception("Enter Password.");
                case "password":
                    throw new Exception("Something Went Wrong, Try Again.");
                case "refresh_token":
                    RefreshToken(parameters);
                    break;
                default:
                    throw new Exception("Invalid grant_type.");
            }
        }

        private void Login(TokenDTO parameters)
        {
            var sessionModel = _userService.IsUserExists(parameters);
            if (sessionModel == null)
            {
                throw new Exception("Invalid Username or Password.");
            }

            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

            var rToken = new RToken
            {
                RefreshToken = refreshToken,
                IsStop = 0,
                UserId = sessionModel.Id,
                CreatedDate = DateTime.Now
            };

            //store the refresh_token
            if (_rTokenService.AddToken(rToken))
            {
                GetJwt(sessionModel, refreshToken);
            }
            else
            {
                throw new Exception("Failed to Add Token.");
            }
        }

        private void RefreshToken(TokenDTO parameters)
        {
            var token = _rTokenService.GetToken(parameters.Refresh_Token);

            if (token == null)
            {
                throw new Exception("Can not refresh token.");
            }

            if (token.IsStop == 1)
            {
                throw new Exception("Refresh token has expired.");
            }

            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

            token.IsStop = 1;
            //expire the old refresh_token and add a new refresh_token
            var updateFlag = _rTokenService.ExpireToken(token);

            var addFlag = _rTokenService.AddToken(new RToken
            {
                RefreshToken = refreshToken,
                IsStop = 0,
                CreatedDate = DateTime.UtcNow,
                UserId = token.UserId
            });

            if (updateFlag && addFlag)
            {
                GetJwt(refreshToken);
            }
            else
            {
                throw new Exception("Can not expire token or a new token");
            }
        }

        private async Task GetJwt(UserDTO sessionModel, string refreshToken)
        {
            var now = DateTime.UtcNow;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, sessionModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),
                new Claim("UserId",Convert.ToString(sessionModel.Id)),
                new Claim("UserName",Convert.ToString(sessionModel.Email))
            };

            var symmetricKeyAsBase64 = _jwtConfig.Value.SecretKey;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var jwt = new JwtSecurityToken(
                issuer: _jwtConfig.Value.Issuer,
                audience: _jwtConfig.Value.Aud,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromHours(24)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = sessionModel.Email ?? "",
                expires_in = (int)TimeSpan.FromHours(24).TotalSeconds,
                refresh_token = refreshToken,
                user_Id = sessionModel.Id
            };

            Request.HttpContext.Response.ContentType = "application/json";
            await Request.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private void GetJwt(string refreshToken)
        {
            var now = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, refreshToken),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
            };

            var symmetricKeyAsBase64 = _jwtConfig.Value.SecretKey;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var jwt = new JwtSecurityToken(
                issuer: _jwtConfig.Value.Issuer,
                audience: _jwtConfig.Value.Aud,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromHours(24)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromHours(24).TotalSeconds,
                refresh_token = refreshToken
            };

            Request.HttpContext.Response.ContentType = "application/json";
            Request.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
    }
}
