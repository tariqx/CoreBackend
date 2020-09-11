using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace CoreBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private const string InvalidRequester = "Invalid Requester";

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("/api/token")]
        public IActionResult RequestToken([FromBody] JObject data)
        {
            dynamic json = JValue.Parse(data.ToString());
            string clientId = json.ClientId;

            //validate if the client is known based on the client ID
            if (clientId == _config["TokenSpecs:ClientId"].ToString())
            {
                var token = new JwtSecurityTokenHandler().WriteToken(TokenGenerator());
                return Ok(token);

            }
            else
            {
                //if no client ID found treat it as a bad request
                return BadRequest(InvalidRequester);
            }
        }

        /// <summary>
        /// Generates a JWT token
        /// </summary>
        /// <returns>JwtSecurityToken</returns>        
        private JwtSecurityToken TokenGenerator()
        {
            //add claims as needed
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "coreapi")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenSpecs:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expMins = _config["TokenSpecs:ExpireInMins"];


            //setup and return a JWT token
            var token = new JwtSecurityToken(
                issuer: _config["TokenSpecs:Issuer"],
                audience: _config["TokenSpecs:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(expMins)),
                signingCredentials: creds);

            return token;
        }
    }
}
