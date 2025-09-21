using Microsoft.EntityFrameworkCore;
using ZimmetApp.Data;

var builder = WebApplication.CreateBuilder(args);
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"[DB-CONFIG] DefaultConnection: {cs ?? "(null)"}");
Console.WriteLine($"[ENV] ASPNETCORE_ENVIRONMENT = {builder.Environment.EnvironmentName}");


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(cs)
       .EnableDetailedErrors()
       .EnableSensitiveDataLogging());

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
       .EnableDetailedErrors()
       .EnableSensitiveDataLogging() 
);

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Persons.Any())
    {
        db.Persons.Add(new ZimmetApp.Models.Person { FullName = "Bilinmiyor" });
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Assignments}/{action=Index}/{id?}");

app.Run();
