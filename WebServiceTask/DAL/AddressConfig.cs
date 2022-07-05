using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebServiceTask.Models;

namespace WebServiceTask.DAL
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).IsRequired().UseIdentityColumn();
            builder.Property(t => t.City).IsRequired().HasMaxLength(100);
            builder.Property(r => r.AddressLine).IsRequired();

            //builder.ToTable("Persons");
        }
    }
}
