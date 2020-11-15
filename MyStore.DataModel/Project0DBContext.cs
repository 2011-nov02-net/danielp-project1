using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MyStore.DataModel
{
    public partial class Project0DBContext : DbContext
    {
        public Project0DBContext()
        {
        }

        public Project0DBContext(DbContextOptions<Project0DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Invintory> Invintories { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers", "Store");

                entity.HasIndex(e => new { e.FirstName, e.LastName, e.MiddleInitial }, "Inv_Name")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.MiddleInitial)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.StoreLocation).HasMaxLength(100);

                entity.HasOne(d => d.StoreLocationNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.StoreLocation)
                    .HasConstraintName("store_FK");
            });

            modelBuilder.Entity<Invintory>(entity =>
            {
                entity.HasKey(e => new { e.StoreLocation, e.ItemName })
                    .HasName("Inv_CPK");

                entity.ToTable("Invintory", "Store");

                entity.Property(e => e.StoreLocation).HasMaxLength(100);

                entity.Property(e => e.ItemName).HasMaxLength(50);

                entity.HasOne(d => d.ItemNameNavigation)
                    .WithMany(p => p.Invintories)
                    .HasForeignKey(d => d.ItemName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvItm_FK");

                entity.HasOne(d => d.StoreLocationNavigation)
                    .WithMany(p => p.Invintories)
                    .HasForeignKey(d => d.StoreLocation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvLoc_FK");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.ItemName)
                    .HasName("Item_pk");

                entity.ToTable("Items", "Store");

                entity.Property(e => e.ItemName).HasMaxLength(50);

                entity.Property(e => e.ItemPrice).HasColumnType("money");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.LocationName)
                    .HasName("PK__Location__F946BB85BE7B4585");

                entity.ToTable("Location", "Store");

                entity.Property(e => e.LocationName).HasMaxLength(100);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders", "Store");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.OrderTime).HasColumnType("datetime");

                entity.Property(e => e.OrderTotal).HasColumnType("money");

                entity.Property(e => e.StoreLocation)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CustomerOrders_FK");

                entity.HasOne(d => d.StoreLocationNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreLocation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StoreOrders_FK");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ItemId })
                    .HasName("ori_PK");

                entity.ToTable("OrderItems", "Store");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(50)
                    .HasColumnName("ItemID");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OrderItemItem_FK");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OrderItemOrder_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
