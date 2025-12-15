using Microsoft.EntityFrameworkCore;
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }           //perform data seeding 
        public DbSet<Role> Roles { get; set; }              //Data Seeding for role based authorization
        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public override int SaveChanges()
        {
            // TAKE A SNAPSHOT → FIXES THE EXCEPTION
            var entries = ChangeTracker
                .Entries()
                .Where(e => !(e.Entity is AuditLog))
                .ToList();   // IMPORTANT FIX

            foreach (var entry in entries)
            {
                // CREATE AUDIT ENTRY
                var log = new AuditLog
                {
                    Action = entry.State.ToString(),
                    EntityName = entry.Entity.GetType().Name,
                    Timestamp = DateTime.Now
                };

                // ADD TO AUDIT LOG TABLE
                base.Add(log);  // use base.Add to avoid modifying Entry collection
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)          //seeding data for hobbies..
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hobby>().HasData(               //Data Seeding for hobbies
                new Hobby { Id = 1, Name = "Cricket" },
                new Hobby { Id = 2, Name = "Music" },
                new Hobby { Id = 3, Name = "Travel" },
                new Hobby { Id = 4, Name = "Reading" }
            );
            modelBuilder.Entity<Role>().HasData(        //Data Seeding for roles
                new Role { Id = 1, Name = "User" },
                new Role { Id = 2, Name = "Admin" },
                new Role { Id = 3, Name = "Manager" },
                new Role { Id = 4, Name = "HR" }
            );
        }


    }
}

