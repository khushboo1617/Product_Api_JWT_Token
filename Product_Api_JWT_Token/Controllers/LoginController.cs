using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Product_Api_JWT_Token.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Product_Api_JWT_Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        private Users AuthenticateUser(Users user)
        {
            Users _user = null;
            if(user.UserEmail == "Test@gmail.com" &&  user.Password == "12345")
            {
                _user = new Users { UserEmail = "Khushboo@gmail.com" };
            }
            return _user;
        }

        private string GenerateToken(Users users)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],_config["Jwt:Audience"],null,
                expires:DateTime.Now.AddMinutes(10),
                signingCredentials:credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users user)
        {
            IActionResult response = Unauthorized();
            var user_ = AuthenticateUser(user);
            if (user_ != null)
            {
                var token = GenerateToken(user_);
                response = Ok(new {token = token});
            }
            return response;
        }
    }
}
