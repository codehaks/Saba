using Microsoft.EntityFrameworkCore;
using Saba.Web.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HafezDbContext>(options =>
{
    options.UseSqlite("Data source=hafezdata.sqlite");
    options.LogTo(Console.WriteLine);
});
var app = builder.Build();



app.MapGet("/", (HafezDbContext db) =>
{
    var row = new Random().Next(1, 999);
    var result = db.Fals.OrderBy(b=>b.Id).Skip(row).First();
    return result.Beit.Split('*');
});

app.Run();
