var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapGet("/cart", () => Results.Ok("ENDPOINT - Shows cart content endpoint"));

app.MapPost("/cart", (ProductModel product) => Results.Ok($"ENDPOINT - Product {product.Name} added to cart"));

app.Run();

class ProductModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
}