﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using InstantGram.Data.DBModels;

namespace InstantGram.Data.DBContexts
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostLike> PostLike { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserFollower> UserFollower { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=PMCLAP1349\\SQLEXPRESS;Database=InstantGram_V001;User Id=sa;Password=India@123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.ContentLink)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UploadOn).HasColumnType("datetime");

                entity.HasOne(d => d.UploadByUser)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.UploadByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.Property(e => e.LikeOn).HasColumnType("datetime");

                entity.HasOne(d => d.LikeByUser)
                    .WithMany(p => p.PostLike)
                    .HasForeignKey(d => d.LikeByUserId);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostLike)
                    .HasForeignKey(d => d.PostId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.DateOfJoining).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserAvatar)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserFollower>(entity =>
            {
                entity.Property(e => e.DateOfFollowing).HasColumnType("datetime");

                entity.HasOne(d => d.FollowingUser)
                    .WithMany(p => p.UserFollowerFollowingUser)
                    .HasForeignKey(d => d.FollowingUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFollowerUser)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
