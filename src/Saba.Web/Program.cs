using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Saba.Web.Data;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HafezDbContext>(options =>
{
    options.UseSqlite("Data source=hafezdata.sqlite");
    options.LogTo(Console.WriteLine);
});
var app = builder.Build();



app.MapGet("/", (HafezDbContext db) =>
{
    var count = db.Fals.Count();
    var row = new Random().Next(1, count - 1);
    var fal = db.Fals.OrderBy(b => b.Id).Skip(row).FirstOrDefault();
    if (fal != null)
    {
        return Results.Ok(fal.Beit?.Split('*'));
    }

    return Results.BadRequest();
});

app.Run();
