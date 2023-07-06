using LMS_Project.Interface;
using LMS_Project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using LMS_Project.Models.Security;
using LMS_Project.ViewModel;
using System.Net;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LMS_Project.Repositories
{
    public class HomeRepo : IHome
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeRepo(ApplicationDbContext context, IConfiguration configuration)  
        {
            _context = context;
            _configuration = configuration;
        }
        ServiceResponse res = new ServiceResponse();


        public async Task<ServiceResponse> Register(RegisterDTO user)
        {
            byte[] passwordHash, passwordSalt;
            EnumsManager.CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);
            var userObjs = new User
            {
                EmailAddress = user.Email,
                FirstName = user.Email,
                LastName = user.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                DateCreated = DateTime.Now,
               
            };

            _context.Add(userObjs);
            int results = await _context.SaveChangesAsync(); 
            if (results > 0)
            {
                res.success = true;
                res.Data = results;
                res.message = "User Registered Successfully!";
                return res;
            }
            res.success = false;
            res.Data = results;
            res.message = "User Registered Failed!";
            return res;


        }  
        public async Task<ServiceResponse> Login(LoginDTO userdto)
        {
            var user = await _context.Users.Where(p => p.EmailAddress == userdto.Email ).FirstOrDefaultAsync();
            if (user != null)
            {
                if (!await VerifyPasswordHash(userdto.Password, user.PasswordHash, user.PasswordSalt))
                {
                    res.message = "Invalid username or password";
                    res.success = false;
                    return res;
                }
                else
                {
                    var obj = new
                    {
                        User = user,
                    };
                   
                   
                    res.message = "User logged in";
                    res.Data = obj;
                    res.success = true;
                    return res;
                }
            }
            else
            {
                res.message = "User Does not exist";
                res.success = false;
                return res;
            }
        }

        private async Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            try
            {
                if (password == null) throw new ArgumentNullException("password");
                if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
                if (passwordHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
                if (passwordSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

                using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
                {
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != passwordHash[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
               // log.Error($"an error occured while verifyPasswordHash method  + {ex.Message} + {ex.InnerException} + {ex.StackTrace}");
                throw;
            }





        } 

        public async Task <bool> IsEmailExist(string Email)
        {
            if(await _context.Users.AnyAsync(f=>f.EmailAddress == Email))
            {
                return true;
            }
            return false;
        }



       

        
    }
}
