//namespace Infrastructure.DbContext2
//{
//    using Domain.DbModels;
//    using Microsoft.EntityFrameworkCore;

//    public class ApplicationDbContext : DbContext
//    {
//        public DbSet<User> Users { get; set; }
//        public DbSet<Report> Reports { get; set; }
//        public DbSet<Role> Roles { get; set; }
//        public DbSet<UserRole> UserRoles { get; set; }
//        public DbSet<ReportRole> ReportRoles { get; set; }

//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);
//            modelBuilder.Entity<User>()
//        .ToTable("Users")
//        .Property(u => u.ID)
//        .HasColumnName("ID"); // Map the property to the correct column

//            modelBuilder.Entity<Role>()
//                .ToTable("Roles")
//                .Property(r => r.ID)
//                .HasColumnName("ID");

//            modelBuilder.Entity<Report>()
//                .ToTable("Reports")
//                .Property(r => r.ID)
//                .HasColumnName("ID");

//            // Composite primary key for UserRoles
//            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

//            // Composite primary key for ReportRoles
//            modelBuilder.Entity<ReportRole>().HasKey(rr => new { rr.ReportId, rr.RoleId });

//            // Define relationships
//            modelBuilder.Entity<UserRole>()
//                .HasOne(ur => ur.User)
//                .WithMany(u => u.UserRoles)
//                .HasForeignKey(ur => ur.UserId);

//            modelBuilder.Entity<UserRole>()
//                .HasOne(ur => ur.Role)
//                .WithMany(r => r.UserRoles)
//                .HasForeignKey(ur => ur.RoleId);

//            modelBuilder.Entity<ReportRole>()
//                .HasOne(rr => rr.Report)
//                .WithMany(r => r.ReportRoles)
//                .HasForeignKey(rr => rr.ReportId);

//            modelBuilder.Entity<ReportRole>()
//                .HasOne(rr => rr.Role)
//                .WithMany(r => r.ReportRoles)
//                .HasForeignKey(rr => rr.RoleId);
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder
//                .UseSqlServer("Server=tcp:dfpowebi-dev.database.windows.net,1433;Initial Catalog=dfpowerbi-dev;Persist Security Info=False;User ID=dfAdmin;Password=dfPowerbi@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
//                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
//        }
//    }


//}
