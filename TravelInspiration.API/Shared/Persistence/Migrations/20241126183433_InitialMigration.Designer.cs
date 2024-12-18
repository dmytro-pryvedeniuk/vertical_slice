﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelInspiration.API.Shared.Persistence;

#nullable disable

namespace TravelInspiration.API.Shared.Persistence.Migrations
{
    [DbContext(typeof(TravelInspirationDbContext))]
    [Migration("20241126183433_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TravelInspiration.API.Shared.Domain.Entities.Itinerary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(2500)
                        .HasColumnType("nvarchar(2500)");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Itineraries", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedBy = "DATASEED",
                            CreatedOn = new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "A wonderful journey through Europe",
                            Name = "European Adventure",
                            UserId = "user1"
                        },
                        new
                        {
                            Id = 2,
                            CreatedBy = "DATASEED",
                            CreatedOn = new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Exploring the wonders of Asia",
                            Name = "Asia Exploration",
                            UserId = "user2"
                        });
                });

            modelBuilder.Entity("TravelInspiration.API.Shared.Domain.Entities.Stop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItineraryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ItineraryId");

                    b.ToTable("Stops", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedBy = "DATASEED",
                            CreatedOn = new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUri = "https://localhost:7120/paris.jpg",
                            ItineraryId = 1,
                            Name = "Paris"
                        },
                        new
                        {
                            Id = 2,
                            CreatedBy = "DATASEED",
                            CreatedOn = new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUri = "https://localhost:7120/rome.jpg",
                            ItineraryId = 1,
                            Name = "Rome"
                        },
                        new
                        {
                            Id = 3,
                            CreatedBy = "DATASEED",
                            CreatedOn = new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUri = "https://localhost:7120/bangkok.jpg",
                            ItineraryId = 2,
                            Name = "Bangkok"
                        },
                        new
                        {
                            Id = 4,
                            CreatedBy = "DATASEED",
                            CreatedOn = new DateTime(2024, 11, 26, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUri = "https://localhost:7120/tokyo.jpg",
                            ItineraryId = 2,
                            Name = "Tokyo"
                        });
                });

            modelBuilder.Entity("TravelInspiration.API.Shared.Domain.Entities.Stop", b =>
                {
                    b.HasOne("TravelInspiration.API.Shared.Domain.Entities.Itinerary", "Itinerary")
                        .WithMany("Stops")
                        .HasForeignKey("ItineraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Itinerary");
                });

            modelBuilder.Entity("TravelInspiration.API.Shared.Domain.Entities.Itinerary", b =>
                {
                    b.Navigation("Stops");
                });
#pragma warning restore 612, 618
        }
    }
}
