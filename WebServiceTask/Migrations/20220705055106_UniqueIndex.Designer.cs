﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebServiceTask.DAL;

namespace WebServiceTask.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220705055106_UniqueIndex")]
    partial class UniqueIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("WebServiceTask.Models.Address", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("AddressLine")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AddressLine")
                        .IsUnique();

                    b.HasIndex("City")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("WebServiceTask.Models.Person", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long?>("AddressId")
                        .HasColumnType("bigint");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique()
                        .HasFilter("[AddressId] IS NOT NULL");

                    b.HasIndex("FirstName")
                        .IsUnique();

                    b.HasIndex("LastName")
                        .IsUnique();

                    b.ToTable("Personal");
                });

            modelBuilder.Entity("WebServiceTask.Models.Person", b =>
                {
                    b.HasOne("WebServiceTask.Models.Address", "Address")
                        .WithOne("Person")
                        .HasForeignKey("WebServiceTask.Models.Person", "AddressId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("WebServiceTask.Models.Address", b =>
                {
                    b.Navigation("Person");
                });
#pragma warning restore 612, 618
        }
    }
}