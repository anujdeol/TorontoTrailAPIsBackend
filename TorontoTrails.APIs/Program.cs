using Microsoft.EntityFrameworkCore;
using TorontoTrails.APIs.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TorontoTrailsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TorontoTrailsConnectionString")));

var app = builder.Build();

// Apply migrations and seed the database before handling requests
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<TorontoTrailsDbContext>();

        dbContext.Database.Migrate();

        await DbInitializer.SeedAsync(dbContext);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database seeding failed: {ex.Message}");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
