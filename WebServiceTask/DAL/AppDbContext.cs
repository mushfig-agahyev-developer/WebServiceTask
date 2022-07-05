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
             On EF6.2, you can use HasIndex() to add indexes for migration through fluent API.

modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            On EF6.1 onwards, you can use IndexAnnotation() to add indexes for migration in your fluent API.

modelBuilder.Entity<User>() 
    .Property(r => r.FirstName) 
    .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));


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
