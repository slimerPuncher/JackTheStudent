﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JackTheStudent.Models
{
    public partial class JackTheStudentContext : DbContext
    {
        public JackTheStudentContext()
        {
        }

        public JackTheStudentContext(DbContextOptions<JackTheStudentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<ClassMaterial> ClassMaterial { get; set; }
        public virtual DbSet<ClassType> ClassType { get; set; }
        public virtual DbSet<Exam> Exam { get; set; }
        public virtual DbSet<Homework> Homework { get; set; }
        public virtual DbSet<LabReport> LabReport { get; set; }
        public virtual DbSet<PersonalReminder> PersonalReminder { get; set; }
        public virtual DbSet<ShortTest> ShortTest { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<GroupProjectMember> GroupProjectMember { get; set; }
        public virtual DbSet<TeamsLink> TeamsLink { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseMySQL(Environment.GetEnvironmentVariable("JACKTHESTUDENT_DB_CON_STRING"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("class");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasColumnName("short_name")
                    .HasMaxLength(10);   
            });

            modelBuilder.Entity<ClassType>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("class_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasColumnName("short_name")
                    .HasMaxLength(10);   
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("group");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasColumnName("group")
                    .HasMaxLength(20); 
            });

            modelBuilder.Entity<TeamsLink>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("teams_link");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClassType)
                    .HasColumnName("class_type")
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info")
                    .HasColumnType("longtext");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasColumnName("class")
                    .HasMaxLength(45);

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnName("date")
                    .HasMaxLength(25);

                entity.Property(e => e.LogById)
                    .IsRequired()
                    .HasColumnName("log_by_id")
                    .HasColumnType("longtext");

                entity.Property(e => e.LogByUsername)
                    .IsRequired()
                    .HasColumnName("log_by_username")
                    .HasColumnType("longtext");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasColumnName("group_id")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("project");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.isGroup)
                    .IsRequired()
                    .HasColumnName("is_group")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasColumnName("group_id")
                    .HasMaxLength(3);                  

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info")
                    .HasColumnType("longtext");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasColumnName("class")
                    .HasMaxLength(45);

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnName("date");

                entity.Property(e => e.LogById)
                    .IsRequired()
                    .HasColumnName("log_by_id")
                    .HasColumnType("longtext");

                entity.Property(e => e.LogByUsername)
                    .IsRequired()
                    .HasColumnName("log_by_username")
                    .HasColumnType("longtext");

                entity.Property(e => e.Materials)
                    .HasColumnName("materials")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<GroupProjectMember>(entity =>
            {
                entity.HasOne(e => e.Project)
                    .WithMany(e => e.GroupProjectMembers)
                    .HasForeignKey(e => e.ProjectId)
                    .IsRequired();                
                
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("group_project_member");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Member)
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<ClassMaterial>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("class_material");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info")
                    .HasColumnType("longtext");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasColumnName("class")
                    .HasMaxLength(45);

                entity.Property(e => e.LogById)
                    .IsRequired()
                    .HasColumnName("log_by_id")
                    .HasColumnType("longtext");

                entity.Property(e => e.LogByUsername)
                    .IsRequired()
                    .HasColumnName("log_by_username")
                    .HasColumnType("longtext");

                entity.Property(e => e.MaterialLink)
                    .IsRequired()
                    .HasColumnName("material_link")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("exam");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info")
                    .HasColumnType("longtext");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasColumnName("class")
                    .HasMaxLength(45);

                entity.Property(e => e.ClassShortName)
                    .IsRequired()
                    .HasColumnName("class_short_name")
                    .HasMaxLength(3);
                
                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnName("date");

                entity.Property(e => e.LogById)
                    .IsRequired()
                    .HasColumnName("log_by_id")
                    .HasColumnType("longtext");

                entity.Property(e => e.LogByUsername)
                    .IsRequired()
                    .HasColumnName("log_by_username")
                    .HasColumnType("longtext");    

                entity.Property(e => e.Materials)
                    .HasColumnName("materials")
                    .HasColumnType("longtext");

                entity.Property(e => e.wasReminded)
                    .HasColumnName("was_reminded")
                    .IsRequired();
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("homework");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info")
                    .HasColumnType("longtext");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasColumnName("class")
                    .HasMaxLength(45);
                
                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnName("date");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasColumnName("group_id");
                
                entity.Property(e => e.LogById)
                    .IsRequired()
                    .HasColumnName("log_by_id")
                    .HasColumnType("longtext");

                entity.Property(e => e.LogByUsername)
                    .IsRequired()
                    .HasColumnName("log_by_username")
                    .HasColumnType("longtext");

                entity.Property(e => e.Materials)
                    .HasColumnName("materials")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<LabReport>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("lab_report");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info")
                    .HasColumnType("longtext");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasColumnName("class")
                    .HasMaxLength(45);

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnName("date");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasColumnName("group_id");
                
                entity.Property(e => e.LogById)
                    .IsRequired()
                    .HasColumnName("log_by_id")
                    .HasColumnType("longtext");

                entity.Property(e => e.LogByUsername)
                    .IsRequired()
                    .HasColumnName("log_by_username")
                    .HasColumnType("longtext");

                entity.Property(e => e.Materials)
                    .HasColumnName("materials")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<PersonalReminder>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("personal_reminder");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.About)
                    .IsRequired()
                    .HasColumnName("about")
                    .HasColumnType("longtext");

                entity.Property(e => e.LogById)
                    .IsRequired()
                    .HasColumnName("log_by_id")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.LogByUsername)
                    .IsRequired()
                    .HasColumnName("log_by_username")
                    .HasColumnType("longtext");

                entity.Property(e => e.SetForDate)
                    .IsRequired()
                    .HasColumnName("set_for_date");

                entity.Property(e => e.ChannelId)
                    .IsRequired()
                    .HasColumnName("channel_id")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.UserMention)
                    .IsRequired()
                    .HasColumnName("user_mention")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ShortTest>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("short_test");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info")
                    .HasColumnType("longtext");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasColumnName("class")
                    .HasMaxLength(45);

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnName("date");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasColumnName("group_id");
                    
                entity.Property(e => e.LogById)
                    .IsRequired()
                    .HasColumnName("log_by_id")
                    .HasColumnType("longtext");

                entity.Property(e => e.LogByUsername)
                    .IsRequired()
                    .HasColumnName("log_by_username")
                    .HasColumnType("longtext");

                entity.Property(e => e.Materials)
                    .HasColumnName("materials")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("test");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info")
                    .HasColumnType("longtext");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasColumnName("class")
                    .HasMaxLength(45);

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnName("date");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasColumnName("group_id");
                    
                entity.Property(e => e.LogById)
                    .IsRequired()
                    .HasColumnName("log_by_id")
                    .HasColumnType("longtext");

                entity.Property(e => e.LogByUsername)
                    .IsRequired()
                    .HasColumnName("log_by_username")
                    .HasColumnType("longtext");

                entity.Property(e => e.Materials)
                    .HasColumnName("materials")
                    .HasColumnType("longtext");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
