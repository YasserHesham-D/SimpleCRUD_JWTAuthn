using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using SimpleCRUD_JWTAuthn.Model.Dto_s;
using SimpleCRUD_JWTAuthn.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleCRUD_JWTAuthn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<User> _userManager,IConfiguration _configuration) : ControllerBase
    {

        [HttpPost("[Action]")]
        public async Task<IActionResult> Register(RegisterDto NewUser)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    UserName = NewUser.UserName,
                    Email = NewUser.Email
                };
                IdentityResult result = await _userManager.CreateAsync(user, NewUser.Password);

                if (result.Succeeded) { return Ok(result); }
                else { foreach (var item in result.Errors) { ModelState.AddModelError("", item.Description); } }
            }

            return BadRequest();
        }
        



        [HttpPost("[Action]")]
        public async Task<IActionResult> Login(LoginDto loginingUser)
        {
            if (ModelState.IsValid)
            {
                User? user = await _userManager.FindByEmailAsync(loginingUser.Email);
                if (user is not null)
                {
                    if (await _userManager.CheckPasswordAsync(user, loginingUser.Password))
                    {

                        // Claims
                        var claims = new List<Claim>();

                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                        claims.Add(new Claim(ClaimTypes.Email, user.Email));

                        claims.Add(new Claim
                            (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        // Roles
                        var roles = await _userManager.GetRolesAsync(user);

                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                        // SigningCredentials
                        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                            (_configuration["JWT:SecretKey"]));

                        var sc = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);

                        // Token
                        var Token = new JwtSecurityToken
                            (
                            claims : claims,
                            issuer:_configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Audience"],
                            expires:DateTime.Now.AddHours(1),
                            signingCredentials: sc
                            );
                        // token wich will be sent to frontend 
                        var anonyObj = new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(Token),
                            expiration = Token.ValidTo,

                        }; 
                        return Ok(anonyObj);
                    }
                    else { ModelState.AddModelError("", "Email Or Password Is Not Valid"); }
                }
                else { ModelState.AddModelError("", "Email Or Password Is Not Valid"); }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("[Action]")]
        [Authorize]
        public async Task<IActionResult> GetRegistersName()
        {
            var Result = _userManager.Users.Select(x=>x.UserName).ToList();

            return Ok(Result);
        }
    }
}
