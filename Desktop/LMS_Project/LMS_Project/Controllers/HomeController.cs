using Azure;
using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace LMS_Project.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHome _home;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public HomeController(ILogger<HomeController> logger, IHome home, IConfiguration configuration, IHttpContextAccessor contextAccessor) : base(contextAccessor.HttpContext)
        {
            _logger = logger;
            _home = home;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        ServiceResponse res = new ServiceResponse();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            var userCheck = await _home.IsEmailExist(register.Email);
            if (userCheck == false)
            {
                var reg = await _home.Register(register);
                if (reg == null)
                {
                   
                    TempData["success"] = "Unable to Register!";
                    res.success = false;
                    return RedirectToAction("Login", "Home");
                }

                TempData["success"] = reg.message;
                res.message = "Registration was Successful!";
                res.success = true;
                return RedirectToAction("Login", "Home");
            }
            TempData["error"] = "User Already Exist!";
            res.message = "User Already Exist!";
            return RedirectToAction("Login","Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO user)
        {



            var result = await _home.Login(user);
            if (result.success == false)
            {

                _logger.LogError($"result from repo when it eqaul to false");
                TempData["error"] = "User Does not Exist!";
                return View();
            }
            else
            {
                var json = JsonConvert.SerializeObject(result.Data);
                var response = JsonConvert.DeserializeObject<LoginResponse>(json);
                var email = response.User.EmailAddress;
                var fullname = response.User.FirstName;


                var claim = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, response.User.Id.ToString()),
                            new Claim(ClaimTypes.Name, response.User.EmailAddress),
                            new Claim("FirstName", response.User.FirstName),

                        };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

                _logger.LogInformation($"key= {key}");
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new JwtSecurityToken
                (
                    claims: claim,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );
                _logger.LogInformation($"tokenDescription got here ");
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenString = tokenHandler.WriteToken(tokenDescriptor);
                var responseData = new AuthResponse
                {
                    Token = tokenString,
                    Email = user.Email,
                    ExpireDate = DateTime.Now.AddMinutes(10),
                    FirstName = fullname,
                };

                _contextAccessor.HttpContext.Session.SetString("EmailAddress", responseData.Email);
                _contextAccessor.HttpContext.Session.SetString("Token", responseData.Token);


                return RedirectToAction("Index", "Media");
            }

        }

        public async Task<IActionResult> LogOut()
        {
            try
            {
                string email = _contextAccessor.HttpContext.Session.GetString("Email");
                string token = _contextAccessor.HttpContext.Session.GetString("Token");
                string password = _contextAccessor.HttpContext.Session.GetString("Password");



                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);



                _contextAccessor.HttpContext.Session.Clear();



                TempData["LogOut"] = "Session expired, Login again";



                return RedirectToAction("Login", "Home");


            }
            catch (Exception ex)
            {
               // _logger.LogError("Error thrown: " + ex.Message.ToString() + " Inner Exception: " + ex.InnerException.ToString() +
                 //  " Stack Trace: " + ex.StackTrace.ToString());
                throw;
            }
        }
    }
}