using DataManagement.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataManagement.Infrastructure;

public class AdvisorConfiguration : IEntityTypeConfiguration<Advisor>
{
    public void Configure(EntityTypeBuilder<Advisor> builder)
    {
        builder.HasKey(a => a.Id);
        builder.OwnsOne(a => a.FullName, fn =>
        {
            fn.Property(f => f.FirstName).IsRequired().HasMaxLength(255);
            fn.Property(f => f.LastName).IsRequired().HasMaxLength(255);
        });
        builder.OwnsOne(a => a.Address, ad => { ad.Property(a => a.Value).HasMaxLength(255); });
        builder.OwnsOne(a => a.Phone, ph => { ph.Property(p => p.Number).HasMaxLength(10); });
        builder.OwnsOne(a => a.SIN, sin => { sin.Property(s => s.Number).IsRequired().HasMaxLength(11); });
        builder.Property(a => a.HealthStatus).HasConversion<string>().IsRequired();
    }
}