using Microsoft.EntityFrameworkCore;
using WebServiceTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
using WebServiceTask.CustomAuditlog;

namespace WebServiceTask.DAL
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _context;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _context = httpContextAccessor;
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

        public DbSet<AuditLog> AuditLogs { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //if (!_context.HttpContext.User.Identity.IsAuthenticated)
            //    return 0;
           
                OnBeforeSaveChanges("_context.HttpContext.User.Identity.Name");
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        public override int SaveChanges()
        {
            //    if (!_context.HttpContext.User.Identity.IsAuthenticated)
            //        return 0;
       
            OnBeforeSaveChanges("_context.HttpContext.User.Identity.Name");
            var result = base.SaveChanges();
            return result;
        }

        private void OnBeforeSaveChanges(string username)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<LogEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new LogEntry(entry, username);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = Enums.AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = Enums.AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = Enums.AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }


}
