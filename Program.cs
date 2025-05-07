using Microsoft.EntityFrameworkCore;
using ProductsApi.Data.Context;
using ProductsApi.Repos;
using ProductsApi.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");

// Add services to the container.
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();

builder.Services.AddDbContext<ApiDbContext>(options => options.UseNpgsql(connectionString));


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API to register products and their categories.", Version = "v1" });
});

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    dbContext.Database.Migrate();
}
catch (Exception ex)
{
    System.Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
}

try
{
    await using var connectoin = new Npgsql.NpgsqlConnection(connectionString);
    await connectoin.OpenAsync();
    System.Console.WriteLine("Connection to PostgreSQL database established successfully.");
}
catch (Exception ex)
{
    System.Console.WriteLine($"An error occurred while connecting to the database: {ex.Message}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API V1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
