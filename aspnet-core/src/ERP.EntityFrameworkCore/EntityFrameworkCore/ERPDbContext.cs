using ERP.Purchase;
using ERP.Entity;
using ERP.Common;
using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Roles;
using ERP.Authorization.Users;
using ERP.Chat;
using ERP.Editions;
using ERP.Friendships;
using ERP.MultiTenancy;
using ERP.MultiTenancy.Accounting;
using ERP.MultiTenancy.Payments;
using ERP.Storage;
using ERP.Configurations;

namespace ERP.EntityFrameworkCore
{
    public class ERPDbContext : AbpZeroDbContext<Tenant, Role, User, ERPDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<ReturnProd> ReturnProds { get; set; }

        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Status> Statuses { get; set; }

        public virtual DbSet<ProductImage> ProductImages { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Brand> Brands { get; set; }

        public virtual DbSet<Discount> Discounts { get; set; }

        public virtual DbSet<Blog> Blogs { get; set; }

        public virtual DbSet<Booking> Bookings { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public ERPDbContext(DbContextOptions<ERPDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReturnProd>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<OrderItem>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Order>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Status>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ProductImage>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            
            modelBuilder.Entity<Product>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            
            modelBuilder.Entity<Category>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Brand>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Discount>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Blog>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Booking>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BinaryObject>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });
            UserConfig.Configure(modelBuilder.Entity<User>());
            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}