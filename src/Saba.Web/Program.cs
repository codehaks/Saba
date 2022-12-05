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

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("slowdown", windowOptions => {
        windowOptions.Window=TimeSpan.FromSeconds(1);
        windowOptions.PermitLimit = 1;
        windowOptions.QueueLimit = 1;
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRateLimiter();

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
        return Results.Ok(fal.Beit?.Split('*'));
    }

    return Results.BadRequest();
}).RequireRateLimiting("slowdown")
.WithOpenApi(); 

app.Run();
