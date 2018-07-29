using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
  public class UserForRegisterDto
  {
    [Required]
    public string Username { get; set; }
    [Required]
    [MinLength(4, ErrorMessage = "Your password should have a minimum of 4 characters")]
    public string Password { get; set; }
  }
}