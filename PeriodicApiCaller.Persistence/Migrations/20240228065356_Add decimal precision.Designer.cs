﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PeriodicApiCaller.Persistence;

#nullable disable

namespace PeriodicApiCaller.Persistence.Migrations
{
    [DbContext(typeof(WeatherInfoDbContext))]
    [Migration("20240228065356_Add decimal precision")]
    partial class Adddecimalprecision
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PeriodicApiCaller.Persistence.Entities.WeatherInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Inserted")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TemperatureC")
                        .HasColumnType("decimal(7, 2)");

                    b.HasKey("Id");

                    b.ToTable("WeatherInfo", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
