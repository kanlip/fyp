using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FYP.Models
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<CorporateProfile> CorporateProfile { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<SkillSet> SkillSet { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }

        // Unable to generate entity type for table 'dbo.Staff_has_Skill_Set'. Please see the warning messages.

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.AppId)
                    .HasName("PK__Appointm__8E2CF7F99905AFE6");

                entity.Property(e => e.AppId).ValueGeneratedNever();

                entity.Property(e => e.AppDate).HasColumnType("date");

                entity.Property(e => e.AppDetail).HasColumnType("text");

                entity.Property(e => e.AppStatus)
                    .IsRequired()
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.CusId).HasColumnName("cus_id");

                entity.Property(e => e.StaffId).HasColumnName("Staff_id");

                entity.HasOne(d => d.Cus)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.CusId)
                    .HasConstraintName("FK__Appointme__cus_i__286302EC");
            });

            modelBuilder.Entity<CorporateProfile>(entity =>
            {
                entity.HasKey(e => e.Cpid)
                    .HasName("PK__Corporat__F5B22BC65C96A86C");

                entity.Property(e => e.Cpid)
                    .HasColumnName("CPId")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.AboutUs)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Defination)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Fblink).HasColumnType("text");

                entity.Property(e => e.Linkinlink).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Services)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Twitlink).HasColumnType("text");

                entity.Property(e => e.Vision)
                    .IsRequired()
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.NextofKin)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AId).HasColumnName("A_Id");

                entity.Property(e => e.CId).HasColumnName("C_Id");

                entity.Property(e => e.CustomerComment).HasColumnType("text");

                entity.Property(e => e.FeedBackTime).HasColumnType("datetime");

                entity.Property(e => e.SId).HasColumnName("S_Id");

                entity.Property(e => e.StaffComment).HasColumnType("text");

                entity.HasOne(d => d.A)
                    .WithMany(p => p.Feedback)
                    .HasForeignKey(d => d.AId)
                    .HasConstraintName("FK__Feedback__A_Id__2C3393D0");

                entity.HasOne(d => d.C)
                    .WithMany(p => p.Feedback)
                    .HasForeignKey(d => d.CId)
                    .HasConstraintName("FK__Feedback__C_Id__2D27B809");
            });

            modelBuilder.Entity<SkillSet>(entity =>
            {
                entity.HasKey(e => e.SkillId)
                    .HasName("PK__SkillSet__DFA09187008DAF30");

                entity.Property(e => e.SkillId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UQ_Email")
                    .IsUnique();

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
                
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnType("varchar(1)");
            });
        }
    }
}