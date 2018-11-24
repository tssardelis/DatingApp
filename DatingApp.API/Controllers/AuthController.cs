using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo,IConfiguration config)
        {
            this.repo=repo;
            _config=config;
        }   

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO regDto)
        {
            regDto.username=regDto.username.ToLower();
            if (await repo.Exists(regDto.username))
                return BadRequest("username exists");
            var usercre=new User{
                Username=regDto.username
            };
            return Ok(await repo.Register(usercre,regDto.password));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var loggedUser=await repo.Login(loginDto.Username.ToLower(),loginDto.Password);
            if (loggedUser==null)
                return Unauthorized();

            var claims=new[]{
                new Claim(ClaimTypes.NameIdentifier,loggedUser.Id.ToString()),
                new Claim(ClaimTypes.Name,loggedUser.Username)
            };

            var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor=new SecurityTokenDescriptor{
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials=creds
            };

            var tokenHandler=new JwtSecurityTokenHandler();
            var token=tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new {
                token=tokenHandler.WriteToken(token)
            });
        }
    }
}