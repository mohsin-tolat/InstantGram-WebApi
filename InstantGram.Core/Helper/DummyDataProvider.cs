using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using InstantGram.Data.DBmodels;
using InstantGram.Data.DbContexts;

namespace InstantGram.Core.Helper
{
    public static class DummyDataProvider
    {

        public static void AddSeedData(ApplicationDbContext context)
        {

            try
            {
                AddPostSeedData(context);
                AddPostLikeSeedData(context);
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddPostSeedData(ApplicationDbContext context)
        {
            var response = ReadFile("DummyPosts.json");
            var seedPostData = JsonConvert.DeserializeObject<List<Post>>(response);
            context.Post.AddRange(seedPostData);
        }

        public static void AddPostLikeSeedData(ApplicationDbContext context)
        {
            var response = ReadFile("DummyPostLike.json");
            var seedPostData = JsonConvert.DeserializeObject<List<PostLike>>(response);
            context.PostLike.AddRange(seedPostData);
        }

        public static string ReadFile(string fileName)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "" + fileName;
            string fileData = string.Empty;
            using (StreamReader str = new StreamReader(filePath))
            {
                fileData = str.ReadToEnd();
            }

            return fileData;
        }
    }
}