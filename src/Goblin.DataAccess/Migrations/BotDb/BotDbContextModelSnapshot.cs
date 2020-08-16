﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Goblin.DataAccess.Migrations.BotDb
{
    [DbContext(typeof(BotDbContext))]
    internal class BotDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                    .HasAnnotation("ProductVersion", "3.1.5")
                    .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Goblin.Domain.Entities.CronJob", b =>
            {
                b.Property<int>("Id")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("integer")
                 .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<long>("ChatId")
                 .HasColumnType("bigint");

                b.Property<int>("ConsumerType")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("integer")
                 .HasDefaultValue(0);

                b.Property<int>("CronType")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("integer")
                 .HasDefaultValue(2);

                b.Property<string>("Name")
                 .IsRequired()
                 .HasColumnType("text");

                b.Property<int>("NarfuGroup")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("integer")
                 .HasDefaultValue(0);

                b.Property<string>("Text")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("character varying(500)")
                 .HasMaxLength(500)
                 .HasDefaultValue("");

                b.Property<string>("WeatherCity")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("text")
                 .HasDefaultValue("");

                b.HasKey("Id");

                b.ToTable("CronJobs");
            });

            modelBuilder.Entity("Goblin.Domain.Entities.Remind", b =>
            {
                b.Property<int>("Id")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("integer")
                 .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<long>("ChatId")
                 .HasColumnType("bigint");

                b.Property<int>("ConsumerType")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("integer")
                 .HasDefaultValue(0);

                b.Property<DateTime>("Date")
                 .HasColumnType("timestamp without time zone");

                b.Property<string>("Text")
                 .IsRequired()
                 .HasColumnType("character varying(100)")
                 .HasMaxLength(100);

                b.HasKey("Id");

                b.ToTable("Reminds");
            });

            modelBuilder.Entity("Goblin.Domain.Entities.TgBotUser", b =>
            {
                b.Property<long>("Id")
                 .HasColumnType("bigint");

                b.Property<bool>("HasScheduleSubscription")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("boolean")
                 .HasDefaultValue(false);

                b.Property<bool>("HasWeatherSubscription")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("boolean")
                 .HasDefaultValue(false);

                b.Property<bool>("IsAdmin")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("boolean")
                 .HasDefaultValue(false);

                b.Property<bool>("IsErrorsEnabled")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("boolean")
                 .HasDefaultValue(true);

                b.Property<int>("NarfuGroup")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("integer")
                 .HasDefaultValue(0);

                b.Property<string>("WeatherCity")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("character varying(100)")
                 .HasMaxLength(100)
                 .HasDefaultValue("");

                b.HasKey("Id");

                b.ToTable("TgBotUsers");
            });

            modelBuilder.Entity("Goblin.Domain.Entities.VkBotUser", b =>
            {
                b.Property<long>("Id")
                 .HasColumnType("bigint");

                b.Property<bool>("HasScheduleSubscription")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("boolean")
                 .HasDefaultValue(false);

                b.Property<bool>("HasWeatherSubscription")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("boolean")
                 .HasDefaultValue(false);

                b.Property<bool>("IsAdmin")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("boolean")
                 .HasDefaultValue(false);

                b.Property<bool>("IsErrorsEnabled")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("boolean")
                 .HasDefaultValue(true);

                b.Property<int>("NarfuGroup")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("integer")
                 .HasDefaultValue(0);

                b.Property<string>("WeatherCity")
                 .ValueGeneratedOnAdd()
                 .HasColumnType("character varying(100)")
                 .HasMaxLength(100)
                 .HasDefaultValue("");

                b.HasKey("Id");

                b.ToTable("VkBotUsers");
            });

            modelBuilder.Entity("Goblin.Domain.Entities.CronJob", b =>
            {
                b.OwnsOne("Goblin.Domain.CronTime", "Time", b1 =>
                {
                    b1.Property<int>("CronJobId")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("integer")
                      .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b1.Property<string>("DayOfMonth")
                      .HasColumnType("text");

                    b1.Property<string>("DayOfWeek")
                      .HasColumnType("text");

                    b1.Property<string>("Hour")
                      .HasColumnType("text");

                    b1.Property<string>("Minute")
                      .HasColumnType("text");

                    b1.Property<string>("Month")
                      .HasColumnType("text");

                    b1.HasKey("CronJobId");

                    b1.ToTable("CronJobs");

                    b1.WithOwner()
                      .HasForeignKey("CronJobId");
                });
            });
#pragma warning restore 612, 618
        }
    }
}