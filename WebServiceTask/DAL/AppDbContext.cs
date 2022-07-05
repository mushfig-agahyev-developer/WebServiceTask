using Microsoft.EntityFrameworkCore;
using WebServiceTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
          

            builder.Entity<Person>().HasOne(v => v.Address)
              .WithOne(p => p.Person).HasForeignKey<Person>(p => p.AddressId);

            builder.Entity<Person>().HasIndex(u => u.FirstName).IsUnique();
            builder.Entity<Person>().HasIndex(u => u.LastName).IsUnique();
            
            builder.Entity<Address>().HasIndex(u => u.City).IsUnique();
            builder.Entity<Address>().HasIndex(u => u.AddressLine).IsUnique();

            /*
            builder.ApplyConfiguration(new PersonConfig());
            builder.ApplyConfiguration(new AddressConfig());
            */

            base.OnModelCreating(builder);
        }


        public DbSet<Person> Personal { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }


}
