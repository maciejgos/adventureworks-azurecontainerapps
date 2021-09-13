using AdventureWorksCore.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdventureWorksCore.Infrastructure.Persistence.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product", "Production");
            builder.Property(e => e.Id).HasColumnName("ProductID").IsRequired();
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.ProductNumber).HasMaxLength(25).IsRequired();
            builder.Property(e => e.MakeFlag).IsRequired();
            builder.Property(e => e.FinishedGoodsFlag).IsRequired();
            builder.Property(e => e.Color).HasMaxLength(15);
            builder.Property(e => e.SafetyStockLevel).IsRequired();
            builder.Property(e => e.ReorderPoint).IsRequired();
            builder.Property(e => e.StandardCost).IsRequired();
            builder.Property(e => e.ListPrice).IsRequired();
            builder.Property(e => e.Size).HasMaxLength(5);
            builder.Property(e => e.SizeUnitMeasureCode).HasMaxLength(3);
            builder.Property(e => e.WeightUnitMeasureCode).HasMaxLength(3);
            builder.Property(e => e.Weight).HasPrecision(8, 2);
            builder.Property(e => e.DaysToManufacture).IsRequired();
            builder.Property(e => e.ProductLine).HasMaxLength(2);
            builder.Property(e => e.Class).HasMaxLength(2);
            builder.Property(e => e.Style).HasMaxLength(2);
            builder.Property(e => e.SellStartDate).IsRequired();
        }
    }
}