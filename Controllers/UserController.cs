using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Register.Model;
using System;
using System.Linq;

namespace Register.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserContext _context;
        public  UserController(IConfiguration config,UserContext context)
        {
            _config = config;
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public ActionResult Create(User user)
        {
            if(_context.Users.Where(u=>u.Email==user.Email).FirstOrDefault() != null)
            {
                return Ok("User Already Exist");
            }
            user.MemberSince = DateTime.Now;
            _context.Users.Add(user);
            _context.SaveChanges();
                return Ok("Success from create method");
        }
        [HttpGet("GetUser")]
        public ActionResult GetValue()
        {
            return Ok("Get Value");
        }
        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public ActionResult Login(Login login)
        {
            var userAvailable = _context.Users.Where(u => u.Email == login.Email && u.Pwd == login.Pwd).FirstOrDefault();
            if(userAvailable != null) {
                return Ok(new JWTService(_config).GenerateToken(
                    userAvailable.UserID.ToString(),
                    userAvailable.FirstName, 
                    userAvailable.LastName,
                    userAvailable.Email,
                    userAvailable.Mobile,
                    userAvailable.Gender
                    ));
            }
            return Ok("Failure");
        }
    }
}
