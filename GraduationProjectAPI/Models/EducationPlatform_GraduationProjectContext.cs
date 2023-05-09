using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GraduationProjectAPI.Models
{
    public partial class EducationPlatform_GraduationProjectContext : DbContext
    {
        public EducationPlatform_GraduationProjectContext()
        {
        }

        public EducationPlatform_GraduationProjectContext(DbContextOptions<EducationPlatform_GraduationProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Content> Contents { get; set; } = null!;
        public virtual DbSet<ContentImage> ContentImages { get; set; } = null!;
        public virtual DbSet<ContentPdf> ContentPdfs { get; set; } = null!;
        public virtual DbSet<ContentVideo> ContentVideos { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Month> Months { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=EducationPlatform_GraduationProject;Trusted_Connection=True; TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasIndex(e => e.ClassIdfk, "IX_Chats_ClassIDFK")
                    .IsUnique();

                entity.Property(e => e.ClassIdfk).HasColumnName("ClassIDFK");

                entity.HasOne(d => d.ClassIdfkNavigation)
                    .WithOne(p => p.Chat)
                    .HasForeignKey<Chat>(d => d.ClassIdfk);
            });

            modelBuilder.Entity<Content>(entity =>
            {
                entity.HasIndex(e => e.ClassId, "IX_Contents_ClassID");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Contents)
                    .HasForeignKey(d => d.ClassId);
            });

            modelBuilder.Entity<ContentImage>(entity =>
            {
                entity.HasIndex(e => e.ContentId, "IX_ContentImages_ContentID");

                entity.Property(e => e.ContentId).HasColumnName("ContentID");

                entity.HasOne(d => d.Content)
                    .WithMany(p => p.ContentImages)
                    .HasForeignKey(d => d.ContentId);
            });

            modelBuilder.Entity<ContentPdf>(entity =>
            {
                entity.ToTable("ContentPDFs");

                entity.HasIndex(e => e.ContentId, "IX_ContentPDFs_ContentID");

                entity.Property(e => e.ContentId).HasColumnName("ContentID");

                entity.HasOne(d => d.Content)
                    .WithMany(p => p.ContentPdfs)
                    .HasForeignKey(d => d.ContentId);
            });

            modelBuilder.Entity<ContentVideo>(entity =>
            {
                entity.HasIndex(e => e.ContentId, "IX_ContentVideos_ContentID");

                entity.Property(e => e.ContentId).HasColumnName("ContentID");

                entity.HasOne(d => d.Content)
                    .WithMany(p => p.ContentVideos)
                    .HasForeignKey(d => d.ContentId);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Mid);

                entity.HasIndex(e => e.ChatId, "IX_Messages_ChatID");

                entity.HasIndex(e => e.StId, "IX_Messages_StID");

                entity.Property(e => e.Mid).HasColumnName("MId");

                entity.Property(e => e.ChatId).HasColumnName("ChatID");

                entity.Property(e => e.StId).HasColumnName("StID");

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ChatId);

                entity.HasOne(d => d.St)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.StId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Month>(entity =>
            {
                entity.HasIndex(e => e.StId, "IX_Months_StID");

                entity.Property(e => e.StId).HasColumnName("StID");

                entity.HasOne(d => d.St)
                    .WithMany(p => p.Months)
                    .HasForeignKey(d => d.StId);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StId);

                entity.HasIndex(e => e.ClassId, "IX_Students_ClassID");

                entity.HasIndex(e => e.IdentityUserId, "IX_Students_IdentityUserID");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.IdentityUserId).HasColumnName("IdentityUserID");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ClassId);

                entity.HasOne(d => d.IdentityUser)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.IdentityUserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
