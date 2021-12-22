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
        public ServiceContext context;
        public ServiceContext replica;
        public Service()
        {
            CreateContext();
            this.footwearRepository = new FootwearRepository(context);
            this.orderRepository = new OrderRepository(context);
            this.reviewRepository = new ReviewRepository(context);
            this.orderItemRepository = new OrderItemRepository(context);
            this.clientRepository = new ClientRepository(context);
        }
        public void CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServiceContext>();
            context = new ServiceContext("Host=localhost;Database=online_shop;Username=postgres;Password=valdorette");
            var result = -1;
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM pg_publication";
                context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                    if (reader.Read())
                        result = reader.GetInt32(0);
                context.Database.CloseConnection();
            }

            if (result == 0)
                context.Database.ExecuteSqlRaw("CREATE PUBLICATION logical_pub FOR ALL TABLES;");

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM pg_replication_slots";
                context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                    if (reader.Read())
                        result = reader.GetInt32(0);
                context.Database.CloseConnection();
            }

            if (result == 0)
                context.Database.ExecuteSqlRaw("SELECT * FROM pg_create_logical_replication_slot('logical_slot', 'pgoutput');");
            replica = new ServiceContext("Host=localhost;Database=new_online_shop;Username=postgres;Password=valdorette");
            replica.CreateSubscription();
        }
    }
    public class ServiceContext : DbContext
    {
        public DbSet<Footwear> footwear { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<OrderItem> order_items { get; set; }
        public DbSet<Client> clients { get; set; }

        public string connectionString { get; set; }
        public ServiceContext(string connectionString)
        {
            this.connectionString = connectionString;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
        public void CreateSubscription()
        {
            var result = -1;
            using (var command = this.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM pg_subscription";
                this.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                    if (reader.Read())
                        result = reader.GetInt32(0);
                this.Database.CloseConnection();
            }
            if (result == 0)
                this.Database.ExecuteSqlRaw("CREATE SUBSCRIPTION logical_sub\n" +
                                            "CONNECTION 'host=localhost port=5432 user=postgres password=valdorette dbname=online_shop'\n" +
                                            "PUBLICATION logical_pub\n" +
                                            "WITH(create_slot = false, slot_name = 'logical_slot');");
        }
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
