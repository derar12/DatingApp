using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]

    // controls and infers datas being passed
    [ApiController]
    public class AuthController : ControllerBase
    
    {
        private readonly IConfiguration _config;
        private readonly IAuthoRepository _repo;
            public AuthController(IAuthoRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost("register")]
        //need [fromBody] if no api controller
        //public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request 
            //need this if you take out apicontroller
          /*  if (!ModelState.IsValid)
                return BadRequest(ModelState);
*/
           userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            // checks if the user exists 
            if(await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate,userForRegisterDto.Password);

           // return CreatedAtRoute() update later just to make it wor  
           return StatusCode(201);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            //user not found
            if(userFromRepo == null){
                //return StatusCode(201); // bug here 
                return Unauthorized();}
                // array to store token 
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };
            //complex security key for token
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            // creating a token tag 
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
