﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using somiod.DAL;

#nullable disable

namespace somiod.Migrations
{
    [DbContext(typeof(InheritanceMappingContext))]
    [Migration("20221215214213_AcurateApplicationRouting")]
    partial class AcurateApplicationRouting
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("somiod.Models.Application", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("creation_dt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("somiod.Models.Module", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("creation_dt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("parentid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("parentid");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("somiod.Models.Module", b =>
                {
                    b.HasOne("somiod.Models.Application", "parent")
                        .WithMany()
                        .HasForeignKey("parentid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("parent");
                });
#pragma warning restore 612, 618
        }
    }
}
