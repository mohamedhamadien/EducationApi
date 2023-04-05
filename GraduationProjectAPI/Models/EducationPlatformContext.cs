using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GraduationProjectAPI.Models
{
    public partial class EducationPlatformContext : DbContext
    {
        public EducationPlatformContext()
        {
        }

        public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Contant> Contants { get; set; } = null!;
        public virtual DbSet<ContantImage> ContantImages { get; set; } = null!;
        public virtual DbSet<ContantPdf> ContantPdfs { get; set; } = null!;
        public virtual DbSet<ContantVideo> ContantVideos { get; set; } = null!;
        public virtual DbSet<Login> Logins { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Month> Months { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=EducationPlatform;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chat");

                entity.Property(e => e.ChatId)
                    .ValueGeneratedNever()
                    .HasColumnName("Chat_ID");

                entity.Property(e => e.Title).HasMaxLength(120);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.ClassId)
                    .ValueGeneratedNever()
                    .HasColumnName("Class_ID");

                entity.Property(e => e.ChatIdfk).HasColumnName("Chat_IDFK");

                entity.Property(e => e.Title).HasMaxLength(120);

                entity.HasOne(d => d.ChatIdfkNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.ChatIdfk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Classes_Chat");
            });

            modelBuilder.Entity<Contant>(entity =>
            {
                entity.ToTable("Contant");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ClassIdfk).HasColumnName("Class_IDFK");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title).HasMaxLength(120);

                entity.HasOne(d => d.ClassIdfkNavigation)
                    .WithMany(p => p.Contants)
                    .HasForeignKey(d => d.ClassIdfk)
                    .HasConstraintName("FK_Contant_Classes");
            });

            modelBuilder.Entity<ContantImage>(entity =>
            {
                entity.ToTable("Contant_Images");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ContantIdfk).HasColumnName("Contant_IDFK");

                entity.HasOne(d => d.ContantIdfkNavigation)
                    .WithMany(p => p.ContantImages)
                    .HasForeignKey(d => d.ContantIdfk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Contant_Images_Contant");
            });

            modelBuilder.Entity<ContantPdf>(entity =>
            {
                entity.ToTable("Contant_PDFs");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ContantIdfk).HasColumnName("Contant_IDFK");

                entity.HasOne(d => d.ContantIdfkNavigation)
                    .WithMany(p => p.ContantPdfs)
                    .HasForeignKey(d => d.ContantIdfk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Contant_PDFs_Contant");
            });

            modelBuilder.Entity<ContantVideo>(entity =>
            {
                entity.ToTable("Contant_Videos");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ContantIdfk).HasColumnName("Contant_IDFK");

                entity.HasOne(d => d.ContantIdfkNavigation)
                    .WithMany(p => p.ContantVideos)
                    .HasForeignKey(d => d.ContantIdfk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Contant_Videos_Contant");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PK_Login_1");

                entity.ToTable("Login");

                entity.Property(e => e.UserName).HasMaxLength(120);

                entity.Property(e => e.Password).HasMaxLength(120);

                entity.Property(e => e.Role).HasMaxLength(50);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MId);

                entity.Property(e => e.MId)
                    .ValueGeneratedNever()
                    .HasColumnName("M_ID");

                entity.Property(e => e.ChatIdfk).HasColumnName("Chat_IDFK");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.StIdfk).HasColumnName("St_IDFK");

                entity.HasOne(d => d.ChatIdfkNavigation)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ChatIdfk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Messages_Chat");

                entity.HasOne(d => d.StIdfkNavigation)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.StIdfk)
                    .HasConstraintName("FK_Messages_Student");
            });

            modelBuilder.Entity<Month>(entity =>
            {
                entity.Property(e => e.MonthId)
                    .ValueGeneratedNever()
                    .HasColumnName("Month_ID");

                entity.Property(e => e.StIdfk).HasColumnName("St_IDFK");

                entity.HasOne(d => d.StIdfkNavigation)
                    .WithMany(p => p.Months)
                    .HasForeignKey(d => d.StIdfk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Months_Student");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StId);

                entity.ToTable("Student");

                entity.Property(e => e.StId)
                    .ValueGeneratedNever()
                    .HasColumnName("St_ID");

                entity.Property(e => e.Address).HasMaxLength(120);

                entity.Property(e => e.ClassIdfk).HasColumnName("Class_IDFK");

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .IsFixedLength();

                entity.Property(e => e.RegistedDate).HasColumnType("date");

                entity.Property(e => e.StName)
                    .HasMaxLength(120)
                    .HasColumnName("St_Name");

                entity.Property(e => e.UserNameFk)
                    .HasMaxLength(120)
                    .HasColumnName("UserNameFK");

                entity.HasOne(d => d.ClassIdfkNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ClassIdfk)
                    .HasConstraintName("FK_Student_Classes");

                entity.HasOne(d => d.UserNameFkNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.UserNameFk)
                    .HasConstraintName("FK_Student_Login");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
