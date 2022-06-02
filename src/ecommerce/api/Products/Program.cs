using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AdventureWorksDB>(opt =>
{
    opt.UseSqlServer(connectionString);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors(options => options.AddPolicy("AnyOrigin", o => o.AllowAnyOrigin()));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseCors();

app.MapGet("/", async (AdventureWorksDB db) => await db.Products.ToListAsync()).RequireCors("AnyOrigin"); ;

app.Run();

class ProductApiModel
{
    public int ProductID { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public decimal ListPrice { get; set; } = 0;
}

class AdventureWorksDB : DbContext
{
    public AdventureWorksDB(DbContextOptions<AdventureWorksDB> options)
        : base(options) { }

    public DbSet<ProductApiModel> Products => Set<ProductApiModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }
}

class ProductConfiguration : IEntityTypeConfiguration<ProductApiModel>
{
    public void Configure(EntityTypeBuilder<ProductApiModel> builder)
    {
        builder.ToTable("Product", "SalesLT");
        builder.HasKey(e => e.ProductID);
        builder.Property(e => e.ProductID).HasColumnName("ProductID").IsRequired();
        builder.Property(e => e.Name).IsRequired();
    }
}