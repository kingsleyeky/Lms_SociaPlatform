using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace LMS_Project.Repositories
{
    public class AccountRepo : IAccount
    {
        private readonly ApplicationDbContext _context;
        ServiceResponse res = new ServiceResponse();


        public AccountRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountDTO>> Index()
        {
            var data = await _context.Accounts.Where(x => x.PlatformId != null).ToListAsync();

            string jsonPage = JsonConvert.SerializeObject(data);
            List<AccountDTO> posts;

            posts = JsonConvert.DeserializeObject<List<AccountDTO>>(jsonPage);

         
            return (posts);
        }

       
     
        public async Task<List<Account>> Get()
        {
            var data = await _context.Accounts.Where(x => x.PlatformId != null).ToListAsync(); 

            if(data.Count > 0)
            {
                return data;
            }
          
            return null;
        }


        public async Task<ServiceResponse> SavePlatform(AccountDTO platform,string Email)
        {

            string name = platform.Name + "." + platform.PlatformName;

            Console.WriteLine(name);
            var data = new Account {Name = name, UserEmail = Email };
            await _context.AddAsync(data);
           var dataSaved =  await _context.SaveChangesAsync();
            
            if(dataSaved>0)
            {
                res.Data= dataSaved;
                res.success = true;
                res.message = "Account Created Successfully";
                return res;
            }
            res.success= false;
            res.Data = null;
            res.message = "Db Error!";
            return res;
        }



        public async Task<ServiceResponse> Edit(AccountDTO account)
        {
            var update = await _context.Accounts.Where(x=>x.PlatformId == account.PlatformId).FirstOrDefaultAsync();
            if(update != null)
            {
                string name = account.Name + "." + account.Name;

                update.Name = name;
                _context.Entry(update).State= EntityState.Modified;
                int result = await _context.SaveChangesAsync();
                if(result>0)
                {
                   
                    res.message = "updated successfully";
                    res.Data = update;
                    res.success = true;
                    return res;
                }
              
                res.message = "update Failed ";
                res.Data = update;
                res.success = false;
                return res;
            }

            else
            {
                res.success = false;
                res.message = "Does not exist";
                res.Data = null;
                return res;
            }
        }  


        public async Task<ServiceResponse> DeleteById(int Id)
        {
            var data = await _context.Accounts.Where(x => x.PlatformId == Id).FirstOrDefaultAsync();
            if(data != null)
            {
                _context.Accounts.Remove(data);
                int result = await _context.SaveChangesAsync(); 
                if(result>0)
                {
                    res.success = true;
                    res.message = "Deleted successfully";
                    return res;
                }
                res.success = false;
                res.message = "db error";
                return res;
            } 

            res.success = false;
            return res;

        }


    }
}
