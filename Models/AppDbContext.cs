using Microsoft.EntityFrameworkCore;
using webmvc.Models.Contact;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace webmvc.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var  tableName = entityType.GetTableName();
                if(tableName.StartsWith("AspNet")){
                    entityType.SetTableName(tableName.Substring(6));
                }
            }


        }

       public DbSet<Contactt> contacts {get;set;}
       public DbSet<AppUser> appUsers {get;set;}

    }
}