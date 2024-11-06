using System.ComponentModel.DataAnnotations;

namespace SimpleCRUD_JWTAuthn.Model.Dto_s
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
       
        [Required]
        public string Password { get; set; }
    }
}
