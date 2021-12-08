using System;
using Microsoft.EntityFrameworkCore;
namespace Model
{
    public class Service
    {
        public FootwearRepository footwearRepository;
        public OrderRepository orderRepository;
        public OrderItemRepository orderItemRepository;
        public ReviewRepository reviewRepository;
        public ClientRepository clientRepository;
        private ServiceContext context;
        public Service()
        {
            this.context = new ServiceContext();
            this.footwearRepository = new FootwearRepository(context);
            this.orderRepository = new OrderRepository(context);
            this.reviewRepository = new ReviewRepository(context);
            this.orderItemRepository = new OrderItemRepository(context);
            this.clientRepository = new ClientRepository(context);
        }
    }
    public class ServiceContext : DbContext
    {
        public DbSet<Footwear> footwear { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<OrderItem> order_items { get; set; }
        public DbSet<Client> clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql($@"Host=localhost;Username=postgres;
            Password=valdorette;Database=online-shop");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasOne(p => p.client)
                .WithMany(t => t.reviews)
                .HasForeignKey(p => p.client_id)
                .HasPrincipalKey(t=>t.id);

            modelBuilder.Entity<Review>()
                .HasOne(p => p.footwear)
                .WithMany(t => t.reviews)
                .HasForeignKey(p => p.product_id)
                .HasPrincipalKey(t=>t.id);

            modelBuilder.Entity<Order>()
                .HasOne(p => p.client)
                .WithMany(t => t.orders)
                .HasForeignKey(p => p.client_id)
                .HasPrincipalKey(t=>t.id);
            
            modelBuilder.Entity<Footwear>()
                .HasMany(c => c.orders)
                .WithMany(s => s.footwear)
                .UsingEntity<OrderItem>(
                    j => j
                    .HasOne(pt => pt.order)
                    .WithMany(t => t.order_items)
                    .HasForeignKey(pt => pt.order_id),
                    j => j
                    .HasOne(pt => pt.footwear)
                    .WithMany(p => p.order_items)
                    .HasForeignKey(pt => pt.product_id));
        }
    }
}
