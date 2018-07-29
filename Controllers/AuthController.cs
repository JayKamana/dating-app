using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

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
        return BadRequest("Username is already taken");

      var userToCreate = new User
      {
        username = userForRegisterDto.Username
      };

      var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

      return StatusCode(201);
    }
  }
}