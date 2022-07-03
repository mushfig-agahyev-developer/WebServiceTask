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
            base.OnModelCreating(builder);

            builder.Entity<Person>().HasOne(v => v.Address)
              .WithOne(p => p.Person).HasForeignKey<Person>(p => p.AddressId).IsRequired(false);
        }


        public DbSet<Person> Personal { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }


}
