using System.Threading.Tasks;
using DatingApp.API.Data;
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
    public async Task<IActionResult> register(string username, string password)
    {
      username = username.ToLower();
      if (await _repo.UserExists(username))
        return BadRequest("Username is already taken");

      var userToCreate = new User
      {
        username = username
      };

      var createdUser = await _repo.Register(userToCreate, password);

      return StatusCode(201);
    }
  }
}