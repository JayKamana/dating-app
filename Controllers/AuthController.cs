using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
  [Route("api/[controller]")]
  public class AuthController : Controller
  {
    public IAuthRepository _repo { get; set; }
    public AuthController(IAuthRepository repo)
    {
      _repo = repo;

    }
    [HttpPost("register")]
    public async Task<IActionResult> register([FromBody]UserForRegisterDto userForRegisterDto)
    {

      userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
      if (await _repo.UserExists(userForRegisterDto.Username))
        ModelState.AddModelError("Username", "Username already exists");

      if (!ModelState.IsValid)
        return BadRequest(ModelState);


      var userToCreate = new User
      {
        username = userForRegisterDto.Username
      };

      var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

      return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]UserForLoginDto userForLoginDto)
    {
      var userFromRepo = await _repo.login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

      if (userFromRepo == null)
        return Unauthorized();

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes("super secret key");
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]{
          new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
          new Claim(ClaimTypes.Name, userFromRepo.username)
    }),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);
      return Ok(new { tokenString });
    }

  }
}