﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using testEfCoreGenericServices;

namespace testEfCoreGenericServices.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200131024843_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("testEfCoreGenericServices.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("user");

                    b.HasData(
                        new
                        {
                            Id = new Guid("cf70c77c-966f-4bda-b0f0-482a5d1062a1"),
                            Name = "abc"
                        },
                        new
                        {
                            Id = new Guid("32feb0dd-6182-4dc0-9ad6-b147b6fb0cbb"),
                            Name = "cde"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}