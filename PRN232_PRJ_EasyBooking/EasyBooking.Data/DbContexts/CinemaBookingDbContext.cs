using System;
using System.Collections.Generic;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EasyBooking.Data.DbContexts;

public partial class CinemaBookingDbContext : DbContext
{
    public CinemaBookingDbContext()
    {
    }

    public CinemaBookingDbContext(DbContextOptions<CinemaBookingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<DiscountCode> DiscountCodes { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<OrderHistory> OrderHistories { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PointConfig> PointConfigs { get; set; }

    public virtual DbSet<PointTransaction> PointTransactions { get; set; }

    public virtual DbSet<RefundHistory> RefundHistories { get; set; }

    public virtual DbSet<RefundPolicy> RefundPolicies { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Showtime> Showtimes { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDiscountCode> UserDiscountCodes { get; set; }

    public virtual DbSet<UserPoints> UserPoints { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("MyCnn")); }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AuditLog__5E548648B68E23BC");

            entity.ToTable("AuditLog");

            entity.Property(e => e.ActionTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Staff).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuditLog__StaffI__236943A5");
        });

        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.HasKey(e => e.CinemaId).HasName("PK__Cinemas__59C926464076A7F1");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
        });

        modelBuilder.Entity<DiscountCode>(entity =>
        {
            entity.ToTable("DiscountCode");

            entity.HasIndex(e => e.Code, "UQ_DiscountCode_Code").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genres__0385057E43CD9AD6");

            entity.HasIndex(e => e.Name, "UQ__Genres__737584F65D788072").IsUnique();

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movies__4BD2941A57E4EF4C");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.PosterUrl).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.VipPercent).HasDefaultValue(0);

            entity.HasMany(d => d.Genres).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("FK__MovieGenr__Genre__4D94879B"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK__MovieGenr__Movie__4CA06362"),
                    j =>
                    {
                        j.HasKey("MovieId", "GenreId").HasName("PK__MovieGen__BBEAC44D94E175B4");
                        j.ToTable("MovieGenres");
                    });
        });

        modelBuilder.Entity<OrderHistory>(entity =>
        {
            entity.HasKey(e => e.OrderHistoryId).HasName("PK__OrderHis__718E6C93804B5CB6");

            entity.ToTable("OrderHistory");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.OrderNote).HasMaxLength(255);
            entity.Property(e => e.OrderStatus).HasMaxLength(50);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);

            entity.HasOne(d => d.Payment).WithMany(p => p.OrderHistories)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderHist__Payme__2645B050");

            entity.HasOne(d => d.User).WithMany(p => p.OrderHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderHist__UserI__2739D489");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38406E3C64");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.PaymentTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TransactionId).HasMaxLength(100);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Payments__UserId__282DF8C2");
        });

        modelBuilder.Entity<PointConfig>(entity =>
        {
            entity.HasKey(e => e.PointConfigId);

            entity.ToTable("PointConfig");

            entity.Property(e => e.ConfigName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.MinAmountToEarn).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
        });

        modelBuilder.Entity<PointTransaction>(entity =>
        {
            entity.ToTable("PointTransaction");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionType).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);

            entity.HasOne(d => d.DiscountCode).WithMany(p => p.PointTransactions)
                .HasForeignKey(d => d.DiscountCodeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PointTransaction_DiscountCode");

            entity.HasOne(d => d.Order).WithMany(p => p.PointTransactions)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PointTransaction_Payments");

            entity.HasOne(d => d.User).WithMany(p => p.PointTransactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_PointTransaction_Users");
        });

        modelBuilder.Entity<RefundHistory>(entity =>
        {
            entity.HasKey(e => e.RefundHistoryId).HasName("PK__RefundHi__B96AD197782A6215");

            entity.ToTable("RefundHistory");

            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.RefundAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RefundTime).HasColumnType("datetime");

            entity.HasOne(d => d.Ticket).WithMany(p => p.RefundHistories)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__RefundHis__Ticke__395884C4");

            entity.HasOne(d => d.User).WithMany(p => p.RefundHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefundHis__UserI__3A4CA8FD");
        });

        modelBuilder.Entity<RefundPolicy>(entity =>
        {
            entity.HasKey(e => e.RefundPolicyId).HasName("PK__RefundPo__378B9DCBA0246679");

            entity.ToTable("RefundPolicy");

            entity.HasIndex(e => e.IsActive, "IX_RefundPolicy_IsActive");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.PolicyName).HasMaxLength(100);
            entity.Property(e => e.RefundPercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Rooms__32863939D938A611");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);

            entity.HasOne(d => d.Cinema).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.CinemaId)
                .HasConstraintName("FK__Rooms__CinemaId__5070F446");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PK__Seats__311713F342BB4894");

            entity.HasIndex(e => new { e.RoomId, e.RowLetter, e.SeatNumber }, "UQ_Seat").IsUnique();

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.RowLetter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SeatType)
                .HasMaxLength(20)
                .HasDefaultValue("Regular");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);

            entity.HasOne(d => d.Room).WithMany(p => p.Seats)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_Seats_Rooms");
        });

        modelBuilder.Entity<Showtime>(entity =>
        {
            entity.HasKey(e => e.ShowtimeId).HasName("PK__Showtime__32D31F208B5EAAFD");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);

            entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK__Showtimes__Movie__5535A963");

            entity.HasOne(d => d.Room).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Showtimes__RoomI__5629CD9C");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC607398CEDA1");

            entity.Property(e => e.BookingTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.RefundAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RefundReason).HasMaxLength(255);
            entity.Property(e => e.RefundTime).HasColumnType("datetime");
            entity.Property(e => e.SeatNumber).HasMaxLength(10);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);

            entity.HasOne(d => d.OrderHistory).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.OrderHistoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Tickets__OrderHi__6C190EBB");

            entity.HasOne(d => d.Showtime).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ShowtimeId)
                .HasConstraintName("FK__Tickets__Showtim__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tickets__UserId__6B24EA82");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C7E6C69A4");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4BA6E4564").IsUnique();

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserDiscountCode>(entity =>
        {
            entity.ToTable("UserDiscountCode");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.RedeemDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UsedDate).HasColumnType("datetime");

            entity.HasOne(d => d.DiscountCode).WithMany(p => p.UserDiscountCodes)
                .HasForeignKey(d => d.DiscountCodeId)
                .HasConstraintName("FK_UserDiscountCode_DiscountCode");

            entity.HasOne(d => d.Order).WithMany(p => p.UserDiscountCodes)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_UserDiscountCode_Payments");

            entity.HasOne(d => d.User).WithMany(p => p.UserDiscountCodes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserDiscountCode_Users");
        });

        modelBuilder.Entity<UserPoints>(entity =>
        {
            entity.HasKey(e => e.UserPointsId);

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteBy).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.UserPoints)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserPoints_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
