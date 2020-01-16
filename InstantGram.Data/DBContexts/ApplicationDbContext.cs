using System;
using Microsoft.EntityFrameworkCore;
using InstantGram.Data.DBmodels;

namespace InstantGram.Data.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
        {
            AddSeedData();
        }

        private void AddSeedData()
        {
            this.AddNewPostsToInMemoryData();
        }

        private void AddNewPostsToInMemoryData()
        {

        }

        public DbSet<Post> Post { get; set; }

        public DbSet<PostLike> PostLike { get; set; }


    }
}