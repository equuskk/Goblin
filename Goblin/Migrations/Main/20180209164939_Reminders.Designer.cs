﻿// <auto-generated />
using Goblin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Goblin.Migrations.Main
{
    [DbContext(typeof(MainContext))]
    [Migration("20180209164939_Reminders")]
    partial class Reminders
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Goblin.Models.Person", b =>
                {
                    b.Property<int>("VkID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<int>("CityID");

                    b.Property<short>("GroupID");

                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Schedule");

                    b.Property<bool>("Weather");

                    b.HasKey("VkID");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Goblin.Models.Remind", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<int>("VkID");

                    b.HasKey("ID");

                    b.ToTable("Reminds");
                });
#pragma warning restore 612, 618
        }
    }
}
