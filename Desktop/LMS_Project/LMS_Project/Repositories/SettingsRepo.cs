using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.Models.Security;
using LMS_Project.ViewModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace LMS_Project.Repositories
{
    public class SettingsRepo:ISettings
    {
        private ApplicationDbContext _dbContext; 

        ServiceResponse res = new ServiceResponse();
        public SettingsRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse> ChangePassWord(ChangePasswordDTO changePassword, string userEmail)
        {
            try
            {
                var Result = await _dbContext.Users.Where(x => x.EmailAddress == userEmail).FirstOrDefaultAsync();
                if (Result != null)
                {
                    if (!await VerifyPasswordHash(changePassword.OldPassword, Result.PasswordHash, Result.PasswordSalt))
                    {
                        res.message = "Incorrect old password";
                        res.success = false;
                        return res;
                    }
                    else
                    {
                        byte[] passwordHash, passwordSalt;
                        EnumsManager.CreatePasswordHash(changePassword.NewPassword, out passwordHash, out passwordSalt);
                        if (!await VerifyPassWordHistory(userEmail, passwordHash, changePassword.NewPassword))
                        {
                            res.message = "kindly use a different new Password!";
                            res.success = false;
                            return res;
                        }
                        Result.PasswordHash = passwordHash;
                        Result.PasswordSalt = passwordSalt;
                        await _dbContext.userHistories.AddAsync(new UserHistory
                        {
                            Email = userEmail,
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt,
                            CreatedDate = DateTime.Now
                        });

                        int results = await _dbContext.SaveChangesAsync();

                        if (results > 0)
                        {
                            res.success= true;
                            res.Data= results;
                            res.message = "Password Changed Successfully!";
                            return res;
                        }
                        res.success = false;
                        res.message = "password did not change,try again leta!";
                        return res;
                    }




                }
                else
                {
                    res.success = false;
                    res.Data = null;
                    res.message = "User does not exist";
                    return res;
                }




            }
            catch (Exception ex)
            {



                throw new Exception("Message=" + ex.Message + ex.InnerException + "Opps! An Error Occured");
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

        public async Task<bool> VerifyPassWordHistory(string userEmail, byte[] passwordHash, string newPassword)
        {
            var passWords = await _dbContext.userHistories.Where(p => p.Email.Trim() == userEmail.Trim()).OrderByDescending(h => h.CreatedDate).ToListAsync();

            if (passWords.Count == 0)
            {
                return true;
            }

            UserHistory latestPassWordUsage = null;
            foreach (var p in passWords)
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512(p.PasswordSalt))
                {



                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newPassword));
                    if (computedHash.SequenceEqual(p.PasswordHash))
                    {
                        latestPassWordUsage = p;
                        break;
                    }
                }
            }

            if (latestPassWordUsage == null)
            {
                return true;
            }

            var newPassWordsAfterWards = passWords.Where(p => p.CreatedDate > latestPassWordUsage.CreatedDate).ToList();
            bool newPassWordsAreValidNew = true;
            foreach (var p in newPassWordsAfterWards)
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512(p.PasswordSalt))
                {
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newPassword));
                    if (computedHash.SequenceEqual(p.PasswordHash))
                    {
                        newPassWordsAreValidNew = false;
                    }
                }
            }
            return (newPassWordsAfterWards?.Count >= 3 && newPassWordsAreValidNew);
        }
    }
}
