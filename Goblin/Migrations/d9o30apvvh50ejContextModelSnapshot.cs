﻿// <auto-generated />

using System;
using Goblin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Goblin.Migrations
{
    [DbContext(typeof(MainContext))]
    internal class d9o30apvvh50ejContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Goblin.Models.Remind", b =>
            {
                b.Property<int>("ID")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("Date");

                b.Property<string>("Text");

                b.Property<long>("VkID");

                b.HasKey("ID");

                b.ToTable("Reminds");
            });

            modelBuilder.Entity("Goblin.Models.User", b =>
            {
                b.Property<int>("ID")
                    .ValueGeneratedOnAdd();

                b.Property<string>("City");

                b.Property<int>("Group")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValue(0);

                b.Property<bool>("Schedule")
                    .HasDefaultValue(false);

                b.Property<long>("Vk");

                b.Property<bool>("Weather")
                    .HasDefaultValue(false);

                b.HasKey("ID");

                b.ToTable("Users");
            });
#pragma warning restore 612, 618
        }
    }
}