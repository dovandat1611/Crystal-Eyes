﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Eyes_Controller.Models;

public partial class CrystalEyesDbContext : DbContext
{
	public CrystalEyesDbContext()
	{
	}

	public CrystalEyesDbContext(DbContextOptions<CrystalEyesDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Admin> Admins { get; set; }

	public virtual DbSet<Category> Categories { get; set; }

	public virtual DbSet<Color> Colors { get; set; }

	public virtual DbSet<Customer> Customers { get; set; }

	public virtual DbSet<ExternalLogin> ExternalLogins { get; set; }

	public virtual DbSet<Feedback> Feedbacks { get; set; }

	public virtual DbSet<Image> Images { get; set; }

	public virtual DbSet<Order> Orders { get; set; }

	public virtual DbSet<OrderDetail> OrderDetails { get; set; }

	public virtual DbSet<Product> Products { get; set; }

	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<UserOtp> UserOtps { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Admin>(entity =>
		{
			entity.HasKey(e => e.UserId).HasName("PK__Admin__B9BE370F00FEF33E");

			entity.ToTable("Admin");

			entity.Property(e => e.UserId)
				.ValueGeneratedNever()
				.HasColumnName("user_id");
			entity.Property(e => e.Dob)
				.HasColumnType("date")
				.HasColumnName("dob");
			entity.Property(e => e.Image)
				.HasMaxLength(255)
				.HasColumnName("image");
			entity.Property(e => e.Name)
				.HasMaxLength(100)
				.HasColumnName("name");
			entity.Property(e => e.Phone)
				.HasMaxLength(15)
				.HasColumnName("phone");

			entity.HasOne(d => d.User).WithOne(p => p.Admin)
				.HasForeignKey<Admin>(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Admin__user_id__32E0915F");
		});

		modelBuilder.Entity<Category>(entity =>
		{
			entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B442B63B6D");

			entity.ToTable("Category");

			entity.Property(e => e.CategoryId).HasColumnName("category_id");
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime")
				.HasColumnName("created_at");
			entity.Property(e => e.IsDelete)
				.HasDefaultValueSql("((0))")
				.HasColumnName("is_delete");
			entity.Property(e => e.Name)
				.HasMaxLength(255)
				.HasColumnName("name");
		});

		modelBuilder.Entity<Color>(entity =>
		{
			entity.HasKey(e => e.ColorId).HasName("PK__Color__1143CECBDF3ADA38");

			entity.ToTable("Color");

			entity.Property(e => e.ColorId).HasColumnName("color_id");
			entity.Property(e => e.ColorName)
				.HasMaxLength(50)
				.HasColumnName("color_name");
			entity.Property(e => e.ColorPath)
				.HasMaxLength(50)
				.HasColumnName("color_path");
			entity.Property(e => e.IsDelete)
				.HasDefaultValueSql("((0))")
				.HasColumnName("is_delete");
			entity.Property(e => e.ProductId).HasColumnName("product_id");

			entity.HasOne(d => d.Product).WithMany(p => p.Colors)
				.HasForeignKey(d => d.ProductId)
				.HasConstraintName("FK__Color__product_i__45F365D3");
		});

		modelBuilder.Entity<Customer>(entity =>
		{
			entity.HasKey(e => e.UserId).HasName("PK__Customer__B9BE370F6F951238");

			entity.ToTable("Customer");

			entity.Property(e => e.UserId)
				.ValueGeneratedNever()
				.HasColumnName("user_id");
			entity.Property(e => e.Address).HasColumnName("address");
			entity.Property(e => e.Dob)
				.HasColumnType("date")
				.HasColumnName("dob");
			entity.Property(e => e.Image).HasColumnName("image");
			entity.Property(e => e.Name)
				.HasMaxLength(100)
				.HasColumnName("name");
			entity.Property(e => e.Phone)
				.HasMaxLength(15)
				.HasColumnName("phone");

			entity.HasOne(d => d.User).WithOne(p => p.Customer)
				.HasForeignKey<Customer>(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Customer__user_i__35BCFE0A");
		});

		modelBuilder.Entity<ExternalLogin>(entity =>
		{
			entity.HasKey(e => e.ExloginId).HasName("PK__External__A73699D95A669FF5");

			entity.ToTable("External_Logins");

			entity.Property(e => e.ExloginId).HasColumnName("exlogin_id");
			entity.Property(e => e.AccessToken).HasColumnName("access_token");
			entity.Property(e => e.ExternalProvider)
				.HasMaxLength(100)
				.HasColumnName("external_provider");
			entity.Property(e => e.ExternalUserId)
				.HasMaxLength(255)
				.HasColumnName("external_user_id");
			entity.Property(e => e.UserId).HasColumnName("user_id");

			entity.HasOne(d => d.User).WithMany(p => p.ExternalLogins)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK__External___user___300424B4");
		});

		modelBuilder.Entity<Feedback>(entity =>
		{
			entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__7A6B2B8CB3CD9BA9");

			entity.ToTable("Feedback");

			entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
			entity.Property(e => e.Content).HasColumnName("content");
			entity.Property(e => e.CreateDate)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime")
				.HasColumnName("create_date");
			entity.Property(e => e.IsDelete)
				.HasDefaultValueSql("((0))")
				.HasColumnName("is_delete");
			entity.Property(e => e.ProductId).HasColumnName("product_id");
			entity.Property(e => e.Star).HasColumnName("star");
			entity.Property(e => e.UserId).HasColumnName("user_id");

			entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks)
				.HasForeignKey(d => d.ProductId)
				.HasConstraintName("FK__Feedback__produc__534D60F1");

			entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK__Feedback__user_i__5441852A");
		});

		modelBuilder.Entity<Image>(entity =>
		{
			entity.HasKey(e => e.ImageId).HasName("PK__Image__DC9AC9551C1FAD98");

			entity.ToTable("Image");

			entity.Property(e => e.ImageId).HasColumnName("image_id");
			entity.Property(e => e.ImageUrl)
				.HasMaxLength(255)
				.HasColumnName("image_url");
			entity.Property(e => e.IsDelete)
				.HasDefaultValueSql("((0))")
				.HasColumnName("is_delete");
			entity.Property(e => e.ProductId).HasColumnName("product_id");

			entity.HasOne(d => d.Product).WithMany(p => p.Images)
				.HasForeignKey(d => d.ProductId)
				.HasConstraintName("FK__Image__product_i__4222D4EF");
		});

		modelBuilder.Entity<Order>(entity =>
		{
			entity.HasKey(e => e.OrderId).HasName("PK__Order__46596229F775EADC");

			entity.ToTable("Order");

			entity.Property(e => e.OrderId).HasColumnName("order_id");
			entity.Property(e => e.AcceptAt)
				.HasColumnType("datetime")
				.HasColumnName("accept_at");
			entity.Property(e => e.AddressReceiver)
				.HasMaxLength(255)
				.HasColumnName("address_receiver");
			entity.Property(e => e.CancelAt)
				.HasColumnType("datetime")
				.HasColumnName("cancel_at");
			entity.Property(e => e.CancelReason).HasColumnName("cancel_reason");
			entity.Property(e => e.ContentReservation).HasColumnName("content_reservation");
			entity.Property(e => e.NameReceiver)
				.HasMaxLength(100)
				.HasColumnName("name_receiver");
			entity.Property(e => e.OrderStatus)
				.HasMaxLength(50)
				.HasColumnName("order_status");
			entity.Property(e => e.PendingAt)
				.HasColumnType("datetime")
				.HasColumnName("pending_at");
			entity.Property(e => e.PhoneReceiver)
				.HasMaxLength(15)
				.HasColumnName("phone_receiver");
			entity.Property(e => e.ShippingAt)
				.HasColumnType("datetime")
				.HasColumnName("shipping_at");
			entity.Property(e => e.SuccessAt)
				.HasColumnType("datetime")
				.HasColumnName("success_at");
			entity.Property(e => e.TotalAmount)
				.HasColumnType("decimal(18, 2)")
				.HasColumnName("total_amount");
			entity.Property(e => e.UserId).HasColumnName("user_id");

			entity.HasOne(d => d.User).WithMany(p => p.Orders)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK__Order__user_id__49C3F6B7");
		});

		modelBuilder.Entity<OrderDetail>(entity =>
		{
			entity.HasKey(e => e.OrderDetailId).HasName("PK__Order_De__3C5A408066995207");

			entity.ToTable("Order_Detail");

			entity.Property(e => e.OrderDetailId).HasColumnName("order_detail_id");
			entity.Property(e => e.ColorId).HasColumnName("color_id");
			entity.Property(e => e.OrderId).HasColumnName("order_id");
			entity.Property(e => e.Price)
				.HasColumnType("decimal(18, 2)")
				.HasColumnName("price");
			entity.Property(e => e.ProductId).HasColumnName("product_id");
			entity.Property(e => e.Quantity).HasColumnName("quantity");
			entity.Property(e => e.TotalPrice)
				.HasComputedColumnSql("([price]*[quantity])", true)
				.HasColumnType("decimal(29, 2)")
				.HasColumnName("total_price");

			entity.HasOne(d => d.Color).WithMany(p => p.OrderDetails)
				.HasForeignKey(d => d.ColorId)
				.HasConstraintName("FK__Order_Det__color__4F7CD00D");

			entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
				.HasForeignKey(d => d.OrderId)
				.HasConstraintName("FK__Order_Det__order__4D94879B");

			entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
				.HasForeignKey(d => d.ProductId)
				.HasConstraintName("FK__Order_Det__produ__4E88ABD4");
		});

		modelBuilder.Entity<Product>(entity =>
		{
			entity.HasKey(e => e.ProductId).HasName("PK__Product__47027DF5049205F2");

			entity.ToTable("Product");

			entity.Property(e => e.ProductId).HasColumnName("product_id");
			entity.Property(e => e.AddInfo).HasColumnName("add_info");
			entity.Property(e => e.CategoryId).HasColumnName("category_id");
			entity.Property(e => e.Description).HasColumnName("description");
			entity.Property(e => e.Discount)
				.HasDefaultValueSql("((0))")
				.HasColumnType("decimal(5, 2)")
				.HasColumnName("discount");
			entity.Property(e => e.IsActive)
				.HasDefaultValueSql("((1))")
				.HasColumnName("is_active");
			entity.Property(e => e.IsDelete)
				.HasDefaultValueSql("((0))")
				.HasColumnName("is_delete");
			entity.Property(e => e.MainImage)
				.HasMaxLength(255)
				.HasColumnName("main_image");
			entity.Property(e => e.Name)
				.HasMaxLength(255)
				.HasColumnName("name");
			entity.Property(e => e.Price)
				.HasColumnType("decimal(18, 2)")
				.HasColumnName("price");
			entity.Property(e => e.SubDescription)
				.HasMaxLength(255)
				.HasColumnName("sub_description");

			entity.HasOne(d => d.Category).WithMany(p => p.Products)
				.HasForeignKey(d => d.CategoryId)
				.HasConstraintName("FK__Product__categor__3C69FB99");
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F256C4141");

			entity.ToTable("User");

			entity.HasIndex(e => e.Email, "UQ__User__AB6E6164F132522D").IsUnique();

			entity.Property(e => e.UserId).HasColumnName("user_id");
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime")
				.HasColumnName("created_at");
			entity.Property(e => e.Email)
				.HasMaxLength(255)
				.HasColumnName("email");
			entity.Property(e => e.IsActive)
				.HasDefaultValueSql("((1))")
				.HasColumnName("is_active");
			entity.Property(e => e.IsExternalLogin)
				.HasDefaultValueSql("((0))")
				.HasColumnName("is_external_login");
			entity.Property(e => e.IsVerify)
				.HasDefaultValueSql("((0))")
				.HasColumnName("is_verify");
			entity.Property(e => e.Password)
				.HasMaxLength(255)
				.HasColumnName("password");
			entity.Property(e => e.RoleName)
				.HasMaxLength(50)
				.HasColumnName("role_name");
		});

		modelBuilder.Entity<UserOtp>(entity =>
		{
			entity.HasKey(e => e.OtpId).HasName("PK__User_OTP__AEE35435B8E6A1E2");

			entity.ToTable("User_OTP");

			entity.Property(e => e.OtpId).HasColumnName("otp_id");
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime")
				.HasColumnName("created_at");
			entity.Property(e => e.ExpiresAt)
				.HasColumnType("datetime")
				.HasColumnName("expires_at");
			entity.Property(e => e.IsUse)
				.HasDefaultValueSql("((0))")
				.HasColumnName("is_use");
			entity.Property(e => e.OtpCode)
				.HasMaxLength(10)
				.HasColumnName("otp_code");
			entity.Property(e => e.UserId).HasColumnName("user_id");

			entity.HasOne(d => d.User).WithMany(p => p.UserOtps)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK__User_OTP__user_i__2B3F6F97");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
