using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pets.Models;

namespace pets.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Tool> Tools => Set<Tool>();
        public DbSet<ToolItem> ToolItems => Set<ToolItem>();

        public DbSet<Owner> Owners => Set<Owner>();
        public DbSet<Pet> Pets => Set<Pet>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // -----------------------------
            // Identity tables (snake_case)
            // -----------------------------
            builder.Entity<User>().ToTable("users");
            builder.Entity<IdentityRole>().ToTable("roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("user_roles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("user_claims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("user_logins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("role_claims");
            builder.Entity<IdentityUserToken<string>>().ToTable("user_tokens");

            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserName).HasColumnName("username");
                entity.Property(e => e.NormalizedUserName).HasColumnName("normalized_username");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.NormalizedEmail).HasColumnName("normalized_email");
                entity.Property(e => e.EmailConfirmed).HasColumnName("email_confirmed");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.SecurityStamp).HasColumnName("security_stamp");
                entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrency_stamp");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
                entity.Property(e => e.TwoFactorEnabled).HasColumnName("two_factor_enabled");
                entity.Property(e => e.LockoutEnd).HasColumnName("lockout_end");
                entity.Property(e => e.LockoutEnabled).HasColumnName("lockout_enabled");
                entity.Property(e => e.AccessFailedCount).HasColumnName("access_failed_count");

                entity.Property(e => e.FullName).HasColumnName("full_name");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.LastLoginAt).HasColumnName("last_login_at");
            });

            // -----------------------------
            // Tool (إذا عندك)
            // -----------------------------
            builder.Entity<Tool>(entity =>
            {
                entity.ToTable("tools");
                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Name).HasColumnName("name");
                entity.Property(x => x.Description).HasColumnName("description");
                entity.Property(x => x.IsActive).HasColumnName("is_active");
            });

            // -----------------------------
            // Owners
            // -----------------------------
            builder.Entity<Owner>(e =>
            {
                e.ToTable("owners");
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.FullName).HasColumnName("full_name");
                e.Property(x => x.Phone).HasColumnName("phone");
                e.Property(x => x.Email).HasColumnName("email");
                e.Property(x => x.Address).HasColumnName("address");
            });

            // -----------------------------
            // Pets
            // -----------------------------
            builder.Entity<Pet>(e =>
            {
                e.ToTable("pets");
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Name).HasColumnName("name");
                e.Property(x => x.Type).HasColumnName("type");
                e.Property(x => x.OwnerId).HasColumnName("owner_id");

                e.HasOne(x => x.Owner)
                 .WithMany()
                 .HasForeignKey(x => x.OwnerId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // -----------------------------
            // Appointments
            // -----------------------------
builder.Entity<Appointment>(e =>
{
    e.ToTable("Appointments");

    e.Property(x => x.Id).HasColumnName("Id");

    e.Property(x => x.OwnerId).HasColumnName("owner_id");
    e.Property(x => x.OwnerName).HasColumnName("owner_name");

    e.Property(x => x.PetId).HasColumnName("pet_id");
    e.Property(x => x.PetName).HasColumnName("pet_name");

    // الدكتور هو اليوزر (Identity)
    e.Property(x => x.DoctorUserId).HasColumnName("doctor_user_id");
    e.Property(x => x.DoctorName).HasColumnName("doctor_name");

    e.Property(x => x.StartAt).HasColumnName("start_at");
    e.Property(x => x.EndAt).HasColumnName("end_at");

    e.Property(x => x.Reason).HasColumnName("Reason");
    e.Property(x => x.Notes).HasColumnName("notes");

    // enum -> int
    e.Property(x => x.Status)
        .HasColumnName("Status")
        .HasConversion<int>();
});

        }
    }
}
