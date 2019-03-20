using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Yetkilim.Web.Models.Ef
{
    public partial class yetkilimDBContext : DbContext
    {
        public yetkilimDBContext()
        {
        }

        public yetkilimDBContext(DbContextOptions<yetkilimDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<CompanyTypes> CompanyTypes { get; set; }
        public virtual DbSet<ExternalUser> ExternalUser { get; set; }
        public virtual DbSet<FeedbackDetail> FeedbackDetail { get; set; }
        public virtual DbSet<FeedbackForms> FeedbackForms { get; set; }
        public virtual DbSet<Feedbacks> Feedbacks { get; set; }
        public virtual DbSet<PanelUser> PanelUser { get; set; }
        public virtual DbSet<Places> Places { get; set; }
        public virtual DbSet<Promotions> Promotions { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=yetkilimDB;Trusted_Connection=True;Integrated Security=true;", x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Companies>(entity =>
            {
                entity.HasOne(d => d.CompanyType)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.CompanyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CompanyTypes>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<FeedbackForms>(entity =>
            {
                entity.HasOne(d => d.Company)
                    .WithMany(p => p.FeedbackForms)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Feedbacks>(entity =>
            {
                entity.HasOne(d => d.Detail)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.DetailId);

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.PlaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<PanelUser>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Surname).HasMaxLength(100);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.PanelUser)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.PanelUser)
                    .HasForeignKey(d => d.PlaceId);
            });

            modelBuilder.Entity<Places>(entity =>
            {
                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Places)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Promotions>(entity =>
            {
                entity.Property(e => e.DueDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.Promotions)
                    .HasForeignKey(d => d.PlaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Promotions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.ExternalUser)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ExternalUserId);
            });
        }
    }
}
