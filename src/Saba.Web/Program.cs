using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Saba.Web.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HafezDbContext>(options =>
{
    options.UseSqlite("Data source=hafezdata.sqlite");
    //options.LogTo(Console.WriteLine);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(1)));
    options.AddPolicy("short", builder => builder.Expire(TimeSpan.FromSeconds(2)));
    options.AddPolicy("long", builder => builder.Expire(TimeSpan.FromSeconds(30)));

});

var app = builder.Build();

app.UseOutputCache();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/fal", (HafezDbContext db,[FromServices]ILogger<Program> logger,HttpContext http) =>
{
    logger.LogInformation("Req-> {mm:s} | {connection}",DateTime.Now, http.Connection.Id);
    var count = db.Fals.Count();
    var row = new Random().Next(1, count - 1);
    var fal = db.Fals.OrderBy(b => b.Id).Skip(row).FirstOrDefault();
    if (fal != null)
    {
        var result = new FalInfo { Beits = fal.Beit?.Split('*'), TimeCreated =TimeOnly.FromDateTime(DateTime.Now) };
        return Results.Ok(result);
    }

    return Results.BadRequest();
}).WithOpenApi().CacheOutput("short"); 

app.Run();

class FalInfo
{
    public string[]? Beits { get; set; }
    public TimeOnly TimeCreated { get; set; }
}