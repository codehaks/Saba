using Microsoft.EntityFrameworkCore;
using Saba.Web.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HafezDbContext>(options =>
{
    options.UseSqlite("Data source=hafezdata.sqlite");
    options.LogTo(Console.WriteLine);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/fal", (HafezDbContext db) =>
{
    var count = db.Fals.Count();
    var row = new Random().Next(1, count - 1);
    var fal = db.Fals.OrderBy(b => b.Id).Skip(row).FirstOrDefault();
    if (fal != null)
    {
        return Results.Ok(fal.Beit?.Split('*'));
    }

    return Results.BadRequest();
}).WithOpenApi(); 

app.Run();
