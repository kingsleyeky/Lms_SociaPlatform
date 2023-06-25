﻿using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LMS_Project.Repositories
{
    public class MediaRepo:IMedia
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDocUpload _doc;
        ServiceResponse res = new ServiceResponse();
        public MediaRepo(ApplicationDbContext dbContext,IDocUpload doc) 
        
        {
          _dbContext= dbContext;
            _doc= doc;
        }

       

        public async Task<ServiceResponse> CreateContent(List<IFormFile> image, PostMediaDTO mediaDTO,string Email)
        {
            if (image.Count > 0)
            {
                var imageResponse = await _doc.ImageUpload(image,"","");
                var mediaCheck = await _dbContext.PostMedias.Where(x => x.UserEmail == Email).FirstOrDefaultAsync(); ;
                if (imageResponse != null)
                {
                    var mediaData = new PostMedia
                    {
                        Content = mediaDTO.Content,
                        ImagePath = imageResponse.Data.ToString(),
                        ScheduledTime = DateTime.Today,
                        VideoLink = mediaDTO.VideoLink,
                        UserEmail = Email,
                        CreatedDate = DateTime.Today,
                        PostPlatforms = mediaDTO.PlatformIds?
                        .Select(platformId => new PostPlatform { PlatformId = platformId })?
                        .ToList()
                    };  

                   await _dbContext.AddAsync(mediaData);
                   _dbContext.SaveChanges();

                    res.message = "saved successfully!";
                    res.Data = mediaData;
                    res.success = true;
                    return res;
                }
                res.success = false;
                res.Data = null;
                res.message = "Failed to Save";
                return res;
            }
               res.message = "image was not uploaded,kindly upload";
               res.success = false;
               return res;
        }

        public async Task<ServiceResponse> Edit(PostMediaDTO account)
        {
            var update = await _dbContext.PostMedias.Where(x => x.Id == account.Id).FirstOrDefaultAsync();
            if (update != null)
            {
                //string name = account.Name + "." + account.Name;

                update.Content = account.Content;
                _dbContext.Entry(update).State = EntityState.Modified;
                int result = await _dbContext.SaveChangesAsync();
                if (result > 0)
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
            var data = await  _dbContext.PostMedias.Where(x => x.Id == Id).FirstOrDefaultAsync();
            //var data2 = await (from s in _dbContext.PostMedias
            //                  join c in _dbContext.PostPlatforms on s.Id equals c.PostId 
            //                  where s.Id == Id
            //                  select c)
            //                .AsNoTracking().ToListAsync();


            if (data != null) 
            { 
                var plat = await _dbContext.PostPlatforms.Where(x=>x.PostId== data.Id).FirstOrDefaultAsync();

                _dbContext.PostMedias.Remove(data); 

                if(plat != null)
                {
                    _dbContext.PostPlatforms.Remove(plat);
                }

                int result = await _dbContext.SaveChangesAsync();
                if (result > 0)
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

        public async Task<List<PostMedia>> Index()
        {
            var data = await _dbContext.PostMedias.Include(x=>x.PostPlatforms).ThenInclude(x=>x.Platform).ToListAsync();


            //var data = await (from s in _dbContext.PostMedias
            //                  join c in _dbContext.Accounts on s.UserEmail equals c.UserEmail
            //                  where s.UserEmail != null
            //                  select new AccountMediaDTO
            //                  {
            //                      Name= c.Name,
            //                      Id = s.Id,
            //                  }).AsNoTracking().ToListAsync();



            //string jsonPage = JsonConvert.SerializeObject(data);
            //List<PostMediaDTO> posts;

            //posts = JsonConvert.DeserializeObject<List<PostMediaDTO>>(jsonPage);


            return data;
        }


        public async Task<List<Tbl_Platform>> GetPlatform()
        {
            return await _dbContext.tbl_platform.ToListAsync();
            
        }

    }
}