using System;
using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.IO;

#nullable disable

namespace DataAccess
{
    public partial class HostelManagementContext : DbContext
    {
        public HostelManagementContext()
        {
        }

        public HostelManagementContext(DbContextOptions<HostelManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillDetail> BillDetails { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Hostel> Hostels { get; set; }
        public virtual DbSet<HostelPic> HostelPics { get; set; }
        public virtual DbSet<IdentityCard> IdentityCards { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Rent> Rents { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomMember> RoomMembers { get; set; }
        public virtual DbSet<RoomPic> RoomPics { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json")
                               .Build();
                var connectionString = configuration.GetConnectionString("HostelManagementContext");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Account__CB9A1CFFD2A1D1E7");

                entity.ToTable("Account");

                entity.HasIndex(e => e.UserEmail, "UQ__Account__D54ADF559A30C3BB")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .HasColumnName("fullName");

                entity.Property(e => e.IdCardNumber)
                    .HasMaxLength(12)
                    .HasColumnName("idCardNumber");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.ProfilePicUrl)
                    .HasMaxLength(256)
                    .HasColumnName("profilePicUrl");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("roleName");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("userEmail");

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("userPassword");

                entity.HasOne(d => d.IdCardNumberNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.IdCardNumber)
                    .HasConstraintName("FK_Account_IdentityCard");
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");

                entity.Property(e => e.BillId).HasColumnName("billId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("createdDate");

                entity.Property(e => e.DueDate)
                    .HasColumnType("date")
                    .HasColumnName("dueDate");

                entity.Property(e => e.EndRentDate)
                    .HasColumnType("date")
                    .HasColumnName("endRentDate");

                entity.Property(e => e.RentId).HasColumnName("rentId");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.StartRentDate)
                    .HasColumnType("date")
                    .HasColumnName("startRentDate");

                entity.HasOne(d => d.Rent)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => new { d.RentId, d.RoomId, d.StartRentDate, d.EndRentDate })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bill_Bill");
            });

            modelBuilder.Entity<BillDetail>(entity =>
            {
                entity.ToTable("BillDetail");

                entity.Property(e => e.BillDetailId).HasColumnName("billDetailId");

                entity.Property(e => e.BillDescription)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("billDescription");

                entity.Property(e => e.BillId).HasColumnName("billId");

                entity.Property(e => e.DateIssued)
                    .HasColumnType("date")
                    .HasColumnName("dateIssued");

                entity.Property(e => e.Fee)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("fee");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillDetails)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BillDetail_Bill");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("categoryName");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("District");

                entity.Property(e => e.DistrictId)
                    .ValueGeneratedNever()
                    .HasColumnName("districtId");

                entity.Property(e => e.DistrictName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("districtName");

                entity.Property(e => e.ProvinceId).HasColumnName("provinceId");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_provinceId");
            });

            modelBuilder.Entity<Hostel>(entity =>
            {
                entity.ToTable("Hostel");

                entity.Property(e => e.HostelId).HasColumnName("hostelId");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.HostelName)
                    .HasMaxLength(100)
                    .HasColumnName("hostelName");

                entity.Property(e => e.HostelOwnerEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("hostelOwnerEmail");

                entity.Property(e => e.LocationId).HasColumnName("locationId");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Hostels)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Hostel_Category");

                entity.HasOne(d => d.HostelOwnerEmailNavigation)
                    .WithMany(p => p.Hostels)
                    .HasPrincipalKey(p => p.UserEmail)
                    .HasForeignKey(d => d.HostelOwnerEmail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hostel_Account");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Hostels)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_locationId");
            });

            modelBuilder.Entity<HostelPic>(entity =>
            {
                entity.HasKey(e => e.HostelPicsId);

                entity.Property(e => e.HostelPicsId).HasColumnName("hostelPicsId");

                entity.Property(e => e.HostelId).HasColumnName("hostelId");

                entity.Property(e => e.HostelPicUrl)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("hostelPicUrl");

                entity.HasOne(d => d.Hostel)
                    .WithMany(p => p.HostelPics)
                    .HasForeignKey(d => d.HostelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HostelPics_Hostel");
            });

            modelBuilder.Entity<IdentityCard>(entity =>
            {
                entity.HasKey(e => e.IdCardNumber);

                entity.ToTable("IdentityCard");

                entity.Property(e => e.IdCardNumber)
                    .HasMaxLength(12)
                    .HasColumnName("idCardNumber");

                entity.Property(e => e.BackIdPicUrl)
                    .HasMaxLength(256)
                    .HasColumnName("backIdPicURL");

                entity.Property(e => e.FrontIdPicUrl)
                    .HasMaxLength(256)
                    .HasColumnName("frontIdPicURL");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.LocationId)
                    .ValueGeneratedNever()
                    .HasColumnName("locationId");

                entity.Property(e => e.AddressString)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("addressString");

                entity.Property(e => e.WardId).HasColumnName("wardId");

                entity.HasOne(d => d.Ward)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.WardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_wardId");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.ToTable("Province");

                entity.Property(e => e.ProvinceId)
                    .ValueGeneratedNever()
                    .HasColumnName("provinceId");

                entity.Property(e => e.ProvinceName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("provinceName");
            });

            modelBuilder.Entity<Rent>(entity =>
            {
                entity.HasKey(e => new { e.RentId, e.RoomId, e.StartRentDate, e.EndRentDate });

                entity.ToTable("Rent");

                entity.Property(e => e.RentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("rentId");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.StartRentDate)
                    .HasColumnType("date")
                    .HasColumnName("startRentDate");

                entity.Property(e => e.EndRentDate)
                    .HasColumnType("date")
                    .HasColumnName("endRentDate");

                entity.Property(e => e.IsDeposited).HasColumnName("isDeposited");

                entity.Property(e => e.RentedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("rentedBy");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("total");

                entity.HasOne(d => d.RentedByNavigation)
                    .WithMany(p => p.Rents)
                    .HasPrincipalKey(p => p.UserEmail)
                    .HasForeignKey(d => d.RentedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rent_Account");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Rents)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rent_Room");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.Deposit)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("deposit");

                entity.Property(e => e.HostelId).HasColumnName("hostelId");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price");

                entity.Property(e => e.RomMaxCapacity).HasColumnName("romMaxCapacity");

                entity.Property(e => e.RoomCurrentCapacity).HasColumnName("roomCurrentCapacity");

                entity.Property(e => e.RoomDescription)
                    .HasMaxLength(500)
                    .HasColumnName("roomDescription");

                entity.Property(e => e.RoomTitle)
                    .HasMaxLength(100)
                    .HasColumnName("roomTitle");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Hostel)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.HostelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_Hostel");
            });

            modelBuilder.Entity<RoomMember>(entity =>
            {
                entity.ToTable("RoomMember");

                entity.Property(e => e.RoomMemberId).HasColumnName("roomMemberId");

                entity.Property(e => e.EndRentDate)
                    .HasColumnType("date")
                    .HasColumnName("endRentDate");

                entity.Property(e => e.IsPresentator).HasColumnName("isPresentator");

                entity.Property(e => e.RentId).HasColumnName("rentId");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.StartRentDate)
                    .HasColumnType("date")
                    .HasColumnName("startRentDate");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("userEmail");

                entity.HasOne(d => d.Rent)
                    .WithMany(p => p.RoomMembers)
                    .HasForeignKey(d => new { d.RentId, d.RoomId, d.StartRentDate, d.EndRentDate })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoomMember_Rent");
            });

            modelBuilder.Entity<RoomPic>(entity =>
            {
                entity.HasKey(e => e.RoomPicsId);

                entity.Property(e => e.RoomPicsId).HasColumnName("roomPicsId");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.RoomPicUrl)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("roomPicUrl");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomPics)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoomPics_Room");
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.ToTable("Ward");

                entity.Property(e => e.WardId)
                    .ValueGeneratedNever()
                    .HasColumnName("wardId");

                entity.Property(e => e.DistrictId).HasColumnName("districtId");

                entity.Property(e => e.WardName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("wardName");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_districtId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
