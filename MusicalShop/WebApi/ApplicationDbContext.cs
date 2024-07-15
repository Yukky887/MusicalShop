using Microsoft.EntityFrameworkCore;
using MusicalShop.Models;

namespace MusicalShop
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<MusicalInstrument> MusicalInstruments { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<PickupPoint> PickupPoints { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MusicalInstrument>()
                 .HasOne(s => s.Brand)
                 .WithMany(b => b.MusicalInstruments)
                 .HasForeignKey(s => s.BrandId);

            modelBuilder.Entity<MusicalInstrument>()
                .HasOne(s => s.Category)
                .WithMany(c => c.MusicalInstruments)
                .HasForeignKey(s => s.CategoryId);

            modelBuilder.Entity<Brand>()
                .HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasOne(s => s.Role)
                .WithMany(s => s.Clients)
                .HasForeignKey(s => s.RoleId);

            modelBuilder.Entity<Role>()
                .HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<Cart>()
                .HasOne(s => s.Client)
                .WithMany(s => s.Carts)
                .HasForeignKey(s => s.ClientId);

            modelBuilder.Entity<Client>()
                .HasIndex(b => b.Username)
                .IsUnique();

            modelBuilder.Entity<Order>()
                 .HasOne(s => s.Client)
                 .WithMany(b => b.Orders)
                 .HasForeignKey(s => s.ClientId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(s => s.Order)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(s => s.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(s => s.MusicalInstrument)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(s => s.MusicalInstrumentId);

            modelBuilder.Entity<Client>()
                .HasIndex(b => b.Username)
                .IsUnique();

            modelBuilder.Entity<Order>()
                 .HasOne(s => s.PickupPoint)
                 .WithMany(b => b.Orders)
                 .HasForeignKey(s => s.PickupPointId);

            modelBuilder.Entity<PickupPoint>()
                .HasIndex(b => b.City)
                .IsUnique();
        }
    }
}
