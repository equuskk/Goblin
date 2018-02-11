﻿// <auto-generated />
using Goblin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Goblin.Models;

namespace Goblin.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20180208130104_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Goblin.Persons", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("ID");

                    b.Property<string>("City");

                    b.Property<int>("CityId")
                        .HasColumnName("CityID");

                    b.Property<short>("GroupId")
                        .HasColumnName("GroupID");

                    b.Property<bool>("Schedule");

                    b.Property<int>("VkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("VkID")
                        .HasDefaultValueSql("0");

                    b.Property<bool>("Weather");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
