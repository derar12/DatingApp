using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    
    {
        private readonly IAuthoRepository _repo;
            public AuthController(IAuthoRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("register")]
        
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request 

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

    }


}