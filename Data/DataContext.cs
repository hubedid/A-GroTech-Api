using A_GroTech_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Data
{
	public class DataContext : DbContext
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<CommodityArea> CommodityAreas { get; set; }
        public DbSet<CommodityType> CommodityTypes { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<DiscussionAnswer> DiscussionAnswers { get; set; }
        public DbSet<DiscussionAnswerImage> DiscussionAnswerImages { get; set; }
        public DbSet<DiscussionImage> DiscussionImages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationImage> NotificationImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PinnedDiscussionAnswer> PinnedDiscussionAnswers { get; set; }
		public DbSet<Prediction> Predictions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserArea> UserAreas { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserArea>()
                .HasKey(ua => new { ua.UserId, ua.AreaId });
            modelBuilder.Entity<UserArea>()
                .HasOne(ua => ua.User)
				.WithMany(u => u.UserAreas)
				.HasForeignKey(ua => ua.UserId);
            modelBuilder.Entity<UserArea>()
				.HasOne(ua => ua.Area)
				.WithMany(a => a.UserAreas)
				.HasForeignKey(ua => ua.AreaId);

			modelBuilder.Entity<CommodityArea>()
				.HasKey(ca => new { ca.CommodityId, ca.AreaId });
            modelBuilder.Entity<CommodityArea>()
				.HasOne(ca => ca.Commodity)
				.WithMany(c => c.CommodityAreas)
				.HasForeignKey(ca => ca.CommodityId);
            modelBuilder.Entity<CommodityArea>()
                .HasOne(ca => ca.Area)
                .WithMany(a => a.CommodityAreas)
                .HasForeignKey(ca => ca.AreaId);

            modelBuilder.Entity<DiscussionAnswerImage>()
                .HasKey(dai => new { dai.DiscussionAnswerId, dai.ImageId });
            modelBuilder.Entity<DiscussionAnswerImage>()
                .HasOne(dai => dai.DiscussionAnswer)
				.WithMany(da => da.DiscussionAnswerImages)
				.HasForeignKey(dai => dai.DiscussionAnswerId);
            modelBuilder.Entity<DiscussionAnswerImage>()
				.HasOne(dai => dai.Image)
				.WithMany(i => i.DiscussionAnswerImages)
				.HasForeignKey(dai => dai.ImageId);

			modelBuilder.Entity<DiscussionImage>()
				.HasKey(di => new { di.DiscussionId, di.ImageId });
			modelBuilder.Entity<DiscussionImage>()
				.HasOne(di => di.Discussion)
                .WithMany(d => d.DiscussionImages)
                .HasForeignKey(di => di.DiscussionId);
            modelBuilder.Entity<DiscussionImage>()
                .HasOne(di => di.Image)
                .WithMany(i => i.DiscussionImages)
                .HasForeignKey(di => di.ImageId);

            modelBuilder.Entity<NotificationImage>()
                .HasKey(ni => new { ni.NotificationId, ni.ImageId });
            modelBuilder.Entity<NotificationImage>()
                .HasOne(ni => ni.Notification)
				.WithMany(n => n.NotificationImages)
				.HasForeignKey(ni => ni.NotificationId);
            modelBuilder.Entity<NotificationImage>()
                .HasOne(ni => ni.Image)
                .WithMany(i => i.NotificationImages)
                .HasForeignKey(ni => ni.ImageId);

            modelBuilder.Entity<ProductImage>()
                .HasKey(pi => new { pi.ProductId, pi.ImageId });
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId);
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Image)
				.WithMany(i => i.ProductImages)
				.HasForeignKey(pi => pi.ImageId);

            modelBuilder.Entity<PinnedDiscussionAnswer>()
				.HasKey(pda => new { pda.DiscussionId, pda.DiscussionAnswerId });
            modelBuilder.Entity<PinnedDiscussionAnswer>()
                .HasOne(pda => pda.Discussion)
                .WithMany(d => d.PinnedDiscussionAnswers)
                .HasForeignKey(pda => pda.DiscussionId);
            modelBuilder.Entity<PinnedDiscussionAnswer>()
                .HasOne(pda => pda.DiscussionAnswer)
				.WithMany(da => da.PinnedDiscussionAnswers)
				.HasForeignKey(pda => pda.DiscussionAnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DiscussionAnswer>()
                .HasOne(da => da.Discussion)
                .WithMany(d => d.DiscussionAnswers)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(o => o.Orders)
                .OnDelete(DeleteBehavior.Restrict);
        }
	}
}
