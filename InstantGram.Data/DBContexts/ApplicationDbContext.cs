using System;
using Microsoft.EntityFrameworkCore;
using InstantGram.Data.DBmodels;

namespace InstantGram.Data.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
        {

        }

        public DbSet<User> User { get; set; }

        public DbSet<Post> Post { get; set; }

        public DbSet<PostLike> PostLike { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                        .HasOne<User>(s => s.User)
                        .WithMany(g => g.Posts)
                        .HasForeignKey(s => s.UploadBy);

            modelBuilder.Entity<PostLike>()
                        .HasOne<Post>(s => s.Post)
                        .WithMany(x => x.PostLikes)
                        .HasForeignKey(s => s.PostId);

            modelBuilder.Entity<PostLike>()
                        .HasOne<User>(s => s.User)
                        .WithMany(x => x.PostLikes)
                        .HasForeignKey(s => s.LikeBy);
        }

    }
}