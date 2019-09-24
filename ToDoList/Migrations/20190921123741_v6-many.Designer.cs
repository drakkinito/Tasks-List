﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDoList.Data;

namespace ToDoList.Migrations
{
    [DbContext(typeof(ListContext))]
    [Migration("20190921123741_v6-many")]
    partial class v6many
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ToDoList.Models.GroupItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("IsPrivate");

                    b.Property<string>("Name");

                    b.Property<string>("UserRole");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ToDoList.Models.TaskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupItemId");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int?>("UserId");

                    b.Property<bool?>("isAssign");

                    b.HasKey("Id");

                    b.HasIndex("GroupItemId");

                    b.HasIndex("UserId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("ToDoList.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ToDoList.Models.UsersGroup", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("GroupItemId");

                    b.HasKey("UserId", "GroupItemId");

                    b.HasIndex("GroupItemId");

                    b.ToTable("UsersGroups");
                });

            modelBuilder.Entity("ToDoList.Models.TaskItem", b =>
                {
                    b.HasOne("ToDoList.Models.GroupItem", "GroupItem")
                        .WithMany("TaskItems")
                        .HasForeignKey("GroupItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ToDoList.Models.User", "Users")
                        .WithMany("TaskItems")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ToDoList.Models.UsersGroup", b =>
                {
                    b.HasOne("ToDoList.Models.GroupItem", "GroupItem")
                        .WithMany("Users")
                        .HasForeignKey("GroupItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ToDoList.Models.User", "User")
                        .WithMany("UsersGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}