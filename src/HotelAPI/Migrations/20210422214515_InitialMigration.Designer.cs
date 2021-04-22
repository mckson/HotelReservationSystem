﻿// <auto-generated />
using System;
using HotelReservation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Migrations
{
    [DbContext(typeof(HotelContext))]
    [Migration("20210422214515_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HotelReservation.Data.Entities.GuestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HotelId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("GuestEntity");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.HotelEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoomsNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Hotels");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.LocationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BuildingNumber")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HotelId")
                        .HasColumnType("int");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HotelId")
                        .IsUnique();

                    b.ToTable("LocationEntity");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.PermissionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("RoleEntityId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleEntityId");

                    b.ToTable("PermissionEntity");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.ReservationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOut")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Deposit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("GuestId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<int>("TotalDays")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("GuestId")
                        .IsUnique();

                    b.HasIndex("RoomId")
                        .IsUnique();

                    b.ToTable("ReservationEntity");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RoleEntity");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RoomEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int>("FloorNumber")
                        .HasColumnType("int");

                    b.Property<int>("HotelId")
                        .HasColumnType("int");

                    b.Property<bool>("IsEmpty")
                        .HasColumnType("bit");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("RoomEntity");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("UserEntity");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.GuestEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.HotelEntity", "Hotel")
                        .WithMany("Guests")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.LocationEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.HotelEntity", "Hotel")
                        .WithOne("Location")
                        .HasForeignKey("HotelReservation.Data.Entities.LocationEntity", "HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.PermissionEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.RoleEntity", null)
                        .WithMany("Permissions")
                        .HasForeignKey("RoleEntityId");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.ReservationEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.GuestEntity", "Guest")
                        .WithOne("Reservation")
                        .HasForeignKey("HotelReservation.Data.Entities.ReservationEntity", "GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelReservation.Data.Entities.RoomEntity", "Room")
                        .WithOne("Reservation")
                        .HasForeignKey("HotelReservation.Data.Entities.ReservationEntity", "RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guest");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RoomEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.HotelEntity", "Hotel")
                        .WithMany("Rooms")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.UserEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.GuestEntity", "Person")
                        .WithOne("User")
                        .HasForeignKey("HotelReservation.Data.Entities.UserEntity", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelReservation.Data.Entities.RoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.GuestEntity", b =>
                {
                    b.Navigation("Reservation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.HotelEntity", b =>
                {
                    b.Navigation("Guests");

                    b.Navigation("Location");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RoleEntity", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RoomEntity", b =>
                {
                    b.Navigation("Reservation");
                });
#pragma warning restore 612, 618
        }
    }
}
