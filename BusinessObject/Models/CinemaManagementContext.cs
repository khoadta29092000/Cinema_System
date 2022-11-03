﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BusinessObject.Models
{
    public partial class CinemaManagementContext : DbContext
    {
        public CinemaManagementContext()
        {
        }

        public CinemaManagementContext(DbContextOptions<CinemaManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Cinema> Cinemas { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<FilmInCinema> FilmInCinemas { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Scheduling> Schedulings { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceInBill> ServiceInBills { get; set; }
        public virtual DbSet<ServiceInCinema> ServiceInCinemas { get; set; }
        public virtual DbSet<Ticked> Tickeds { get; set; }
        public virtual DbSet<TypeInFilm> TypeInFilms { get; set; }
        public virtual DbSet<Types> Types { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("workstation id=CinemaManagement.mssql.somee.com;packet size=4096;user id=tiensidien1234_SQLLogin_1;pwd=nv6a752x2u;data source=CinemaManagement.mssql.somee.com;persist security info=False;initial catalog=CinemaManagement; Encrypt=false;TrustServerCertificate=true"
                //optionsBuilder.UseSqlServer("Server=DESKTOP-B626C3N\\DUONGMH;TrustServerCertificate=True;Database=Cinema Management;Uid=sa;password=root;"
                );
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Avatar).HasMaxLength(4000);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FK_Account_Cinema");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");

                entity.Property(e => e.CouponId).HasMaxLength(50);

                entity.Property(e => e.Time).HasColumnType("datetime");



                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Bill_Account");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.CouponId)
                    .HasConstraintName("FK_Bill_Coupon");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_Bill_Payment");
            });

            modelBuilder.Entity<Cinema>(entity =>
            {
                entity.ToTable("Cinema");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Image).HasMaxLength(4000);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Cinemas)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Cinema_Location");
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.ToTable("Coupon");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.ToTable("Film");

                entity.Property(e => e.Actor).HasMaxLength(500);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Director).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(4000);

                entity.Property(e => e.Language).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.Property(e => e.Trailer).HasMaxLength(4000);
            });

            modelBuilder.Entity<FilmInCinema>(entity =>
            {
                entity.ToTable("FilmInCinema");

                entity.HasKey(e => new { e.FilmId, e.CinemaId });

                entity.Property(e => e.Endtime).HasColumnType("date");

                entity.Property(e => e.Startime).HasColumnType("date");

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.FilmInCinemas)
                    .HasForeignKey(d => d.CinemaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FilmInCinema_Cinema");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.FilmInCinemas)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FilmInCinema_Film");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FK_Room_Cinema");
            });

            modelBuilder.Entity<Scheduling>(entity =>
            {
                entity.ToTable("Scheduling");

                entity.Property(e => e.Date).HasColumnType("date");
                entity.Property(e => e.Date).HasColumnType("StartTime");
                entity.Property(e => e.Date).HasColumnType("EndTime");


                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.Schedulings)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FK_Scheduling_Cinema");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Schedulings)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Scheduling_Room");

                entity.HasOne(d => d.FilmInCinema)
                    .WithMany(p => p.Schedulings)
                    .HasForeignKey(d => new { d.FilmId, d.CinemaId })
                    .HasConstraintName("FK_Scheduling_FilmInCinema");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.ToTable("Seat");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Seats)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Seat_Room");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Image).HasMaxLength(4000);

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<ServiceInBill>(entity =>
            {
                entity.ToTable("ServiceInBill");

                entity.HasKey(e => new { e.ServiceInCinemaId, e.BillId });

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.ServiceInBills)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceInBill_Bill");

                entity.HasOne(d => d.ServiceInCinema)
                    .WithMany(p => p.ServiceInBills)
                    .HasForeignKey(d => d.ServiceInCinemaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceInBill_ServiceInCinema");
            });

            modelBuilder.Entity<ServiceInCinema>(entity =>
            {
                entity.ToTable("ServiceInCinema");

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.ServiceInCinemas)
                    .HasForeignKey(d => d.CinemaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceInCinema_Cinema");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceInCinemas)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceInCinema_Service1");
            });

            modelBuilder.Entity<Ticked>(entity =>
            {
                entity.ToTable("Ticked");

                entity.HasKey(e => new { e.SeatId, e.BillId, e.SchedulingId, e.AccountId });

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Tickeds)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticked_Account");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.Tickeds)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticked_Bill");

                entity.HasOne(d => d.Seat)
                    .WithMany(p => p.Tickeds)
                    .HasForeignKey(d => d.SeatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticked_Seat");
            });

            modelBuilder.Entity<TypeInFilm>(entity =>
            {
                entity.ToTable("TypeInFilm");

                entity.HasKey(e => new { e.TypeId, e.FilmId });

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.TypeInFilms)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TypeInFilm_Film");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.TypeInFilms)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TypeInFilm_Type");
            });

            modelBuilder.Entity<Types>(entity =>
            {
                entity.ToTable("Types");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}