﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SkillSystem.Infrastructure.Persistence;

#nullable disable

namespace SkillSystem.Infrastructure.Migrations
{
    [DbContext(typeof(SkillSystemDbContext))]
    [Migration("20230320153200_AddSalaryMigration")]
    partial class AddSalaryMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SkillSystem.Core.Entities.Duty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Duties");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.EmployeeSkill", b =>
                {
                    b.Property<string>("EmployeeId")
                        .HasColumnType("text");

                    b.Property<int>("SkillId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("boolean");

                    b.HasKey("EmployeeId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("EmployeeSkills");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("PrevGradeId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PrevGradeId")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.GradeSkill", b =>
                {
                    b.Property<int>("GradeId")
                        .HasColumnType("integer");

                    b.Property<int>("SkillId")
                        .HasColumnType("integer");

                    b.HasKey("GradeId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("GradeSkills");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.ManagerSubordinate", b =>
                {
                    b.Property<Guid>("ManagerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubordinateId")
                        .HasColumnType("uuid");

                    b.HasKey("ManagerId", "SubordinateId");

                    b.ToTable("ManagersSubordinates");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.PositionDuty", b =>
                {
                    b.Property<int>("PositionId")
                        .HasColumnType("integer");

                    b.Property<int>("DutyId")
                        .HasColumnType("integer");

                    b.HasKey("PositionId", "DutyId");

                    b.HasIndex("DutyId");

                    b.ToTable("PositionDuties");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.PositionGrade", b =>
                {
                    b.Property<int>("PositionId")
                        .HasColumnType("integer");

                    b.Property<int>("GradeId")
                        .HasColumnType("integer");

                    b.HasKey("PositionId", "GradeId");

                    b.HasIndex("GradeId");

                    b.ToTable("PositionGrades");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Salary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Bonus")
                        .HasColumnType("numeric");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Rate")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Wage")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Salaries");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.EmployeeSkill", b =>
                {
                    b.HasOne("SkillSystem.Core.Entities.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Grade", b =>
                {
                    b.HasOne("SkillSystem.Core.Entities.Grade", "PrevGrade")
                        .WithOne("NextGrade")
                        .HasForeignKey("SkillSystem.Core.Entities.Grade", "PrevGradeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("SkillSystem.Core.Entities.Role", "Role")
                        .WithMany("Grades")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PrevGrade");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.GradeSkill", b =>
                {
                    b.HasOne("SkillSystem.Core.Entities.Grade", "Grade")
                        .WithMany("GradeSkills")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkillSystem.Core.Entities.Skill", "Skill")
                        .WithMany("GradeSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Grade");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.PositionDuty", b =>
                {
                    b.HasOne("SkillSystem.Core.Entities.Duty", "Duty")
                        .WithMany("PositionDuties")
                        .HasForeignKey("DutyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SkillSystem.Core.Entities.Position", "Position")
                        .WithMany("PositionDuties")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Duty");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.PositionGrade", b =>
                {
                    b.HasOne("SkillSystem.Core.Entities.Grade", "Grade")
                        .WithMany("PositionGrades")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SkillSystem.Core.Entities.Position", "Position")
                        .WithMany("PositionGrades")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grade");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Skill", b =>
                {
                    b.HasOne("SkillSystem.Core.Entities.Skill", "Group")
                        .WithMany("SubSkills")
                        .HasForeignKey("GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Duty", b =>
                {
                    b.Navigation("PositionDuties");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Grade", b =>
                {
                    b.Navigation("GradeSkills");

                    b.Navigation("NextGrade");

                    b.Navigation("PositionGrades");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Position", b =>
                {
                    b.Navigation("PositionDuties");

                    b.Navigation("PositionGrades");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Role", b =>
                {
                    b.Navigation("Grades");
                });

            modelBuilder.Entity("SkillSystem.Core.Entities.Skill", b =>
                {
                    b.Navigation("GradeSkills");

                    b.Navigation("SubSkills");
                });
#pragma warning restore 612, 618
        }
    }
}
