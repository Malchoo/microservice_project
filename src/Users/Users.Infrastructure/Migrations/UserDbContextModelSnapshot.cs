﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Users.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UserId");

                    b.ComplexProperty<Dictionary<string, object>>("Contacts", "Users.Domain.Entities.User.Contacts#Contacts", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasMaxLength(254)
                                .HasColumnType("nvarchar(254)")
                                .HasColumnName("Email");

                            b1.Property<string>("MobileNumber")
                                .IsRequired()
                                .HasMaxLength(15)
                                .HasColumnType("nvarchar(15)")
                                .HasColumnName("MobileNumber");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Preferences", "Users.Domain.Entities.User.Preferences#Preferences", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Currency")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("tinyint")
                                .HasDefaultValue(0)
                                .HasColumnName("Currency");

                            b1.Property<int>("Language")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("tinyint")
                                .HasDefaultValue(0)
                                .HasColumnName("Language");

                            b1.Property<int>("Theme")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("tinyint")
                                .HasDefaultValue(0)
                                .HasColumnName("Theme");

                            b1.Property<int>("Title")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("tinyint")
                                .HasDefaultValue(0)
                                .HasColumnName("Title");

                            b1.Property<int>("TwoFactorAuth")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("tinyint")
                                .HasDefaultValue(1)
                                .HasColumnName("TwoFactorAuth");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Profile", "Users.Domain.Entities.User.Profile#Profile", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)")
                                .HasColumnName("FirstName");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)")
                                .HasColumnName("LastName");

                            b1.Property<string>("MiddleName")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)")
                                .HasColumnName("MiddleName");

                            b1.Property<Guid>("RegistrationId")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("RegistrationId");

                            b1.Property<string>("Username")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)")
                                .HasColumnName("Username");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Settings", "Users.Domain.Entities.User.Settings#Settings", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<bool>("IsBlockedByAdmin")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bit")
                                .HasDefaultValue(false)
                                .HasColumnName("IsBlockedByAdmin");

                            b1.Property<bool>("IsDeleted")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bit")
                                .HasDefaultValue(false)
                                .HasColumnName("IsDeleted");

                            b1.Property<bool>("IsVerified")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bit")
                                .HasDefaultValue(true)
                                .HasColumnName("IsVerified");
                        });

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Users.Infrastructure.IntegrationEvents.OutboxIntegrationEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EventContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxIntegrationEventsConfigurations", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
